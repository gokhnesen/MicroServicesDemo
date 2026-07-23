
using MicroServicesDemo.Data;
using Microsoft.EntityFrameworkCore;

namespace MicroServicesDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<Data.AppDbContext>(options =>
                options.UseInMemoryDatabase("InMem"));

            builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseRouting();

            PrepDb.PrepPopulation(app);

            app.Run();
        }
    }
}
