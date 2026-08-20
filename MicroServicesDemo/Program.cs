
using MicroServicesDemo.AsyncDataServices;
using MicroServicesDemo.Data;
using MicroServicesDemo.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;
using SyncDataServices.Http.Grpc;

namespace MicroServicesDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Console.WriteLine("--> Using SqlServer Db");
            builder.Services.AddDbContext<Data.AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
            builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();
            builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
            builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
            builder.Services.AddGrpc();

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            Console.WriteLine($"CommandService URL: {builder.Configuration["CommandService"]}");

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();
            app.MapGrpcService<GrpcPlatformService>();
            app.MapGet("/protos/platform.proto", async context =>
            {
                await context.Response.WriteAsync(await File.ReadAllTextAsync("Protos/platform.proto"));
            });

            PrepDb.PrepPopulation(app);

            app.Run();
        }
    }
}
