using Delivery.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Delivery.API.Infrastructure.Factories
{
    //For EF Core migration
    public class DeliveryDbContextFactory : IDesignTimeDbContextFactory<DeliveryContext>
    {
        public DeliveryContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DeliveryContext>();

            optionsBuilder.UseSqlServer(config["ConnectionString"], sqlServerOptionsAction: o => o.MigrationsAssembly("Delivery.API"));

            return new DeliveryContext(optionsBuilder.Options);
        }
    }
}