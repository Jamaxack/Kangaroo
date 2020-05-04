﻿using Delivery.Domain.AggregatesModel.ClientAggregate;
using Delivery.Domain.AggregatesModel.DeliveryOrderAggregate;
using Delivery.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.API.Infrastructure
{
    public class DeliveryContextSeed
    {
        public async Task SeedAsync(DeliveryContext context, IWebHostEnvironment env)
        {
            var policy = CreatePolicy(nameof(DeliveryContextSeed));
            await policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    if (!context.DeliveryOrderStatuses.Any())
                    {
                        context.DeliveryOrderStatuses.AddRange(GetPredefinedDeliveryOrderStatus());
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

                    if (!context.DeliveryOrders.Any())
                    {
                        var client = context.Clients.Single();
                        var order = new DeliveryOrder(client.Id, 1, 2, "Just note");
                        context.DeliveryOrders.Add(order);

                        var contactPersonPickup = new ContactPerson("PickupPerson", "+111111111");
                        var contactPersonDropoff = new ContactPerson("DropoffPerson", "+222222222");
                        var deliveryLocationPickUp = new DeliveryLocation("31, Amid", "0", "0", "0", "0", 0, 0, "RC cola", DateTime.Now, DateTime.Now.AddMinutes(30), null, contactPersonPickup);
                        var deliveryLocationDropOff = new DeliveryLocation("31, Yagondka street", "17", "2", "4", "24", 0, 0, "Note", DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), null, contactPersonDropoff);

                        order.SetPickUpLocation(deliveryLocationPickUp);
                        order.SetDropOffLocation(deliveryLocationDropOff);
                        await context.SaveEntitiesAsync();
                    }
                    await context.SaveEntitiesAsync();
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