using Delivering.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Delivering.API.Infrastructure.Factories
{
    //For EF Core migration
    public class DeliveringDbContextFactory : IDesignTimeDbContextFactory<DeliveringContext>
    {
        public DeliveringContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DeliveringContext>();

            optionsBuilder.UseSqlServer(config["ConnectionString"], sqlServerOptionsAction: o => o.MigrationsAssembly("Delivering.API"));

            return new DeliveringContext(optionsBuilder.Options);
        }
    }
}