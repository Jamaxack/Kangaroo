using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Order.Domain.AggregatesModel.ClientAggregate;
using Order.Domain.AggregatesModel.CourierAggregate;
using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using Order.Infrastructure;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Infrastructure
{
    public class DeliveryOrderContextSeed
    {
        public async Task SeedAsync(DeliveryOrderContext context, IWebHostEnvironment env)
        {
            var policy = CreatePolicy(nameof(DeliveryOrderContextSeed));
            await policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    if (!context.DeliveryOrderStatuses.Any())
                    {
                        context.DeliveryOrderStatuses.AddRange(GetPredefinedDeliveryOrderStatus());
                        await context.SaveChangesAsync();
                    }

                    if (!context.DeliveryLocationActions.Any())
                    {
                        context.DeliveryLocationActions.AddRange(GetPredefinedDeliveryLocationActions());
                        await context.SaveChangesAsync();
                    }
                    //TODO: Only for demo purposes
                    if (!context.Clients.Any())
                    {
                        var identityGuid = Guid.NewGuid();
                        var client = new Client(identityGuid, "ClientFirstName", "ClientLastName", "+123456789");
                        context.Clients.Add(client);
                        await context.SaveChangesAsync();
                    }

                    if (!context.Couriers.Any())
                    {
                        var identityGuid = Guid.NewGuid();
                        var client = new Courier(identityGuid, "CourierFirstName", "CourierLastName", "+999999999");
                        context.Couriers.Add(client);
                        await context.SaveChangesAsync();
                    }

                    if (!context.DeliveryOrders.Any())
                    {
                        var client = context.Clients.Single();
                        var settings = new DeliveryOrderNotificationSettings(true, true);
                        var deliveryOrder = new DeliveryOrder(client.Id, 1, 8, 1, "Just note", settings);
                        context.DeliveryOrders.Add(deliveryOrder);
                        await context.SaveChangesAsync();
                    }

                    if (!context.DeliveryLocations.Any())
                    {
                        var order = context.DeliveryOrders.Include(x => x.DeliveryOrderStatus).Single();
                        var contactPersonPickup = new ContactPerson("PickupPerson", "+111111111");
                        var contactPersonDropoff = new ContactPerson("DropoffPerson", "+222222222");
                        order.AddDelivaryLocation("31, Amid", "0", "0", "0", "0", 0, 0, "RC cola", 11, 0, false, DeliveryLocationAction.PickUp.Id, DateTime.Now, DateTime.Now.AddMinutes(30), null, contactPersonPickup);
                        order.AddDelivaryLocation("31, Yagondka street", "17", "2", "4", "24", 0, 0, "Note", 0, 20, true, DeliveryLocationAction.DropOff.Id, DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), null, contactPersonDropoff);
                        await context.SaveChangesAsync();
                    }
                };
            });
        }

        IEnumerable<DeliveryOrderStatus> GetPredefinedDeliveryOrderStatus()
        {
            return new List<DeliveryOrderStatus>()
            {
                DeliveryOrderStatus.New,
                DeliveryOrderStatus.Available,
                DeliveryOrderStatus.CourierAssigned,
                DeliveryOrderStatus.CourierDeparted,
                DeliveryOrderStatus.CourierPickedUp,
                DeliveryOrderStatus.CourierArrived,
                DeliveryOrderStatus.Completed,
                DeliveryOrderStatus.Reactivated,
                DeliveryOrderStatus.Canceled,
                DeliveryOrderStatus.Delayed,
                DeliveryOrderStatus.Failed
            };
        }

        IEnumerable<DeliveryLocationAction> GetPredefinedDeliveryLocationActions()
        {
            return new List<DeliveryLocationAction>()
            {
                DeliveryLocationAction.PickUp,
                DeliveryLocationAction.DropOff
            };
        }

        AsyncRetryPolicy CreatePolicy(string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        //TODO: Write warning to logger
                        Console.WriteLine(exception);
                    }
                );
        }
    }
}
