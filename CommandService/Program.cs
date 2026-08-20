
using CommandsService.AsyncDataServices;
using CommandsService.Data;
using CommandsService.EventProcessing;
using Microsoft.EntityFrameworkCore;
using SyncDataServices.Grpc;

namespace CommandService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemoryDb"));
            builder.Services.AddScoped<ICommandRepo, CommandRepo>();

            builder.Services.AddControllers();
            builder.Services.AddHostedService<MessageBusSubscriber>();
            builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsProduction())
            {
                app.UseSwagger(c => c.RouteTemplate = "commands/swagger/{documentName}/swagger.json");
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "commands/swagger";
                    c.SwaggerEndpoint("/commands/swagger/v1/swagger.json", "Commands API V1");
                });
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            PrebDb.PrepPopulation(app);

            app.Run();
        }
    }
}
