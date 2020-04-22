using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Application.Queries
{
    public class DeliveryOrderQueries : IDeliveryOrderQueries
    {
        string _connectionString = string.Empty;
        public DeliveryOrderQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<DeliveryOrder> GetDeliveryOrderAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<dynamic>(
                  @"select o.[Id] as id,o.Number as number,
                        os.Name as deliveryOrderStatus,
                        oi.Address as address, oi.Note as note
                    FROM [Order].DeliveryOrders o
                        LEFT JOIN [Order].DeliveryLocations oi ON o.Id = oi.DeliveryOrderId 
                        LEFT JOIN [Order].DeliveryOrderStatus os on o.DeliveryOrderStatusId = os.Id
                    WHERE o.Id=@id", new { id });

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return MapOrderItems(result);
            }
        }

        DeliveryOrder MapOrderItems(dynamic result)
        {
            var order = new DeliveryOrder
            {
                number = result[0].number,
                deliveryOrderStatus = result[0].deliveryOrderStatus,
                deliveryLocations = new List<DeliveryLocation>(),
            };

            foreach (dynamic item in result)
            {
                var orderitem = new DeliveryLocation
                {
                    address = item.address,
                    note = item.note,
                };

                order.deliveryLocations.Add(orderitem);
            }

            return order;
        }
    }
}
