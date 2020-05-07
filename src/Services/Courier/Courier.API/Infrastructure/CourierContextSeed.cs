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

                var delivery = new Delivery()
                {
                    Number = 1,
                    CourierId = courier.Id,
                    Price = 12,
                    Note = "Amid Store",
                    Weight = 2,
                    DeliveryStatus = DeliveryStatus.NotStarted,
                    PickUpLocation = new DeliveryLocation()
                    {
                        Address = "31mkr Amid store",
                        ContactPerson = new ContactPerson()
                        {
                            Name = "PickUpPerson",
                            Phone = "+123321123"
                        }
                    },
                    DropOffLocation = new DeliveryLocation()
                    {
                        Address = "31/17/24",
                        ContactPerson = new ContactPerson()
                        {
                            Name = "DropOffPerson",
                            Phone = "+999888777"
                        }
                    },
                };
                await context.Deliveries.InsertOneAsync(delivery);
            }
        }
    }
}
