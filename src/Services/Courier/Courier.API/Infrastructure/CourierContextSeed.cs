using Courier.API.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure
{
    public class CourierContextSeed
    {
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory)
        {
            var config = applicationBuilder.ApplicationServices.GetRequiredService<IOptions<CourierSettings>>();
            var context = new CourierContext(config);
            if (!context.Couriers.Database.GetCollection<Model.Courier>("Couriers").AsQueryable().Any())
            {
                var courier = new Model.Courier()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Phone = "+112233445566"
                };
                await context.Couriers.InsertOneAsync(courier);

                var courierLocation = new CourierLocation()
                {
                    CourierId = courier.Id,
                    DateTime = DateTime.UtcNow,
                    Latitude = 0.1223,
                    Longitude = 1.2111
                };
                await context.CourierLocations.InsertOneAsync(courierLocation);
            }
        }
    }
}
