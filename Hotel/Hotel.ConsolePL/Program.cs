using Hotel.DAL;
using Hotel.DAL.DbSeeder;
using Hotel.DAL.Interfaces;
using Hotel.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hotel.ConsolePL
{
    public class Program
    {
        public ServiceProvider ConfigureServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<HotelDataContext>()
                .AddScoped<IDbSeeder, DbSeeder>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IDbSeeder>();

                initializer.Seed();
            }

            return serviceProvider;
        }
        public static void Main(string[] args)
        {
            var app = new Program();
            var serviceProvider = app.ConfigureServices();
        }
    }

}
