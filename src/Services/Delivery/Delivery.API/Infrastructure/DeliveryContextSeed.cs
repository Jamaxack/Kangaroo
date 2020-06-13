using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delivery.Domain.AggregatesModel.ClientAggregate;
using Delivery.Domain.AggregatesModel.DeliveryAggregate;
using Delivery.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Polly;
using Polly.Retry;

namespace Delivery.API.Infrastructure
{
    public class DeliveryContextSeed
    {
        public async Task SeedAsync(DeliveryContext context, IWebHostEnvironment webHostEnvironment)
        {
            var policy = CreatePolicy(nameof(DeliveryContextSeed));
            await policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    if (!context.DeliveryStatuses.Any())
                    {
                        context.DeliveryStatuses.AddRange(GetPredefinedDeliveryStatus());
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

                    await context.SaveEntitiesAsync();
                }
            });
        }

        private IEnumerable<DeliveryStatus> GetPredefinedDeliveryStatus()
        {
            return new List<DeliveryStatus>
            {
                DeliveryStatus.New,
                DeliveryStatus.Available,
                DeliveryStatus.CourierAssigned,
                DeliveryStatus.CourierDeparted,
                DeliveryStatus.CourierPickedUp,
                DeliveryStatus.CourierArrived,
                DeliveryStatus.Completed,
                DeliveryStatus.Reactivated,
                DeliveryStatus.Canceled,
                DeliveryStatus.Delayed,
                DeliveryStatus.Failed
            };
        }

        private AsyncRetryPolicy CreatePolicy(string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(
                retries,
                retry => TimeSpan.FromSeconds(5),
                (exception, timeSpan, retry, ctx) =>
                {
                    //TODO: Write warning to logger
                    Console.WriteLine(exception);
                }
            );
        }
    }
}