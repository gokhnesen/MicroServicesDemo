
using MicroServicesDemo.Data;
using MicroServicesDemo.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            Console.WriteLine($"CommandService URL: {builder.Configuration["CommandService"]}");

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseRouting();

            PrepDb.PrepPopulation(app);

            app.Run();
        }
    }
}
