using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Application.Queries
{
    public class DeliveryOrderQueries : IDeliveryOrderQueries
    {
        #region SqlQueries
        const string SelectOrdersQuery =
            @"SELECT 
	                do.[Id], 
	                do.[Number],
	                do.[CreatedDateTime],
	                do.[FinishedDateTime],
	                do.[PaymentAmount],
	                do.[InsuranceAmount],
	                do.[Weight],
	                do.[Note],
	                do.[DeliveryOrderNotificationSettings_ShouldNotifySenderOnOrderStatusChange] as ShouldNotifySenderOnOrderStatusChange,
	                do.[DeliveryOrderNotificationSettings_ShouldNotifyRecipientOnOrderStatusChange] as ShouldNotifyRecipientOnOrderStatusChange,
	                do.[ClientId],
	                do.[CourierId],
	                dos.[Name] as DeliveryOrderStatus,
	                dla.[Name] as DeliveryLocationAction,
	                dl.[Id],
	                dl.[Address],
	                dl.[BuildingNumber],
	                dl.[EnterenceNumber],
	                dl.[FloorNumber],
	                dl.[ApartmentNumber],
	                dl.[Latitude],
	                dl.[Longitude],
	                dl.[Note],
	                dl.[BuyoutAmount],
	                dl.[TakingAmount],
	                dl.[IsPaymentInThisDeliveryLocation],
	                dl.[ContactPerson_Name] as ContactPersonName,
	                dl.[ContactPerson_Phone] as ContactPersonPhone,
	                dl.[ArrivalStartDateTime],
	                dl.[ArrivalFinishDateTime],
	                dl.[CourierArrivedDateTime]
                FROM
	                [Order].DeliveryOrders do
	                LEFT JOIN [Order].DeliveryLocations dl ON do.Id = dl.DeliveryOrderId
	                LEFT JOIN [Order].DeliveryOrderStatus dos ON do.DeliveryOrderStatusId = dos.Id
	                LEFT JOIN [Order].DeliveryLocationActions dla ON dl.DeliveryLocationActionId = dla.Id";

        const string SelectOrderByIdSqlQuery =
            @"SELECT TOP(1)
                    do.[Id],
                       [Number],
                       [CreatedDateTime],
                       [FinishedDateTime],
                       [PaymentAmount],
                       [InsuranceAmount],
                       [Weight],
                       [Note],
                       [DeliveryOrderNotificationSettings_ShouldNotifySenderOnOrderStatusChange] as ShouldNotifySenderOnOrderStatusChange,
                       [DeliveryOrderNotificationSettings_ShouldNotifyRecipientOnOrderStatusChange] as ShouldNotifyRecipientOnOrderStatusChange,
                       [ClientId],
                       [CourierId],
	                   dos.[Name] as DeliveryOrderStatus
                  FROM [Kangaroo.Services.Order].[Order].[DeliveryOrders] as do
                  LEFT JOIN [Order].DeliveryOrderStatus dos ON dos.Id = do.DeliveryOrderStatusId
                  WHERE do.[Id] = @orderId;
   
                SELECT
                      dl.[Id],
                      [Address],
                      [BuildingNumber],
                      [EnterenceNumber],
                      [FloorNumber],
                      [ApartmentNumber],
                      [Latitude],
                      [Longitude],
                      [Note],
                      [BuyoutAmount],
                      [TakingAmount],
                      [IsPaymentInThisDeliveryLocation], 
                      [ContactPerson_Name] as ContactPersonName,
                      [ContactPerson_Phone] as ContactPersonPhone,
                      [ArrivalStartDateTime],
                      [ArrivalFinishDateTime],
                      [CourierArrivedDateTime],
	                  dla.[Name] as DeliveryLocationAction
                  FROM [Order].[DeliveryLocations] as dl
                  LEFT JOIN [Order].DeliveryLocationActions dla ON dla.Id = dl.DeliveryLocationActionId 
                  WHERE dl.[DeliveryOrderId] = @orderId";
        #endregion

        string _connectionString = string.Empty;
        public DeliveryOrderQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<List<DeliveryOrderViewModel>> GetDeliveryOrdersAsync(int pageSize, int pageIndex)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var offset = pageSize * pageIndex;
                var pagedQuery = $"{SelectOrdersQuery} {@"ORDER BY do.[Number] OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY"}";

                var deliveryOrders = new Dictionary<Guid, DeliveryOrderViewModel>();
                await connection.QueryAsync<DeliveryOrderViewModel, DeliveryLocationViewModel, DeliveryOrderViewModel>(pagedQuery,
                 (deliveryOrder, deliveryLocation) =>
                 {
                     if (!deliveryOrders.TryGetValue(deliveryOrder.Id, out DeliveryOrderViewModel deliveryOrderEntry))
                     {
                         deliveryOrderEntry = deliveryOrder;
                         deliveryOrders.Add(deliveryOrderEntry.Id, deliveryOrderEntry);
                     }

                     deliveryOrderEntry.DeliveryLocations.Add(deliveryLocation);
                     return deliveryOrderEntry;
                 }, param: new { offset, pageSize });

                var result = deliveryOrders.Values.ToList();

                return result;
            }
        }

        public async Task<List<DeliveryOrderViewModel>> GetDeliveryOrdersByClientIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var deliveryOrders = new Dictionary<Guid, DeliveryOrderViewModel>();
                connection.Open();
                await connection.QueryAsync<DeliveryOrderViewModel, DeliveryLocationViewModel, DeliveryOrderViewModel>(
                 $"{SelectOrdersQuery} WHERE do.ClientId=@id",
                 (deliveryOrder, deliveryLocation) =>
                 {
                     if (!deliveryOrders.TryGetValue(deliveryOrder.Id, out DeliveryOrderViewModel deliveryOrderEntry))
                     {
                         deliveryOrderEntry = deliveryOrder;
                         deliveryOrders.Add(deliveryOrderEntry.Id, deliveryOrderEntry);
                     }

                     deliveryOrderEntry.DeliveryLocations.Add(deliveryLocation);
                     return deliveryOrderEntry;
                 }, new { id });

                var result = deliveryOrders.Values.ToList();

                if (result.Count == 0)
                    throw new KeyNotFoundException();

                return result;
            }
        }

        public async Task<List<DeliveryOrderViewModel>> GetDeliveryOrdersByCourierIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var deliveryOrders = new Dictionary<Guid, DeliveryOrderViewModel>();
                connection.Open();
                await connection.QueryAsync<DeliveryOrderViewModel, DeliveryLocationViewModel, DeliveryOrderViewModel>(
               $"{SelectOrdersQuery} WHERE do.CourierId=@id",
                 (deliveryOrder, deliveryLocation) =>
                 {
                     if (!deliveryOrders.TryGetValue(deliveryOrder.Id, out DeliveryOrderViewModel deliveryOrderEntry))
                     {
                         deliveryOrderEntry = deliveryOrder;
                         deliveryOrders.Add(deliveryOrderEntry.Id, deliveryOrderEntry);
                     }

                     deliveryOrderEntry.DeliveryLocations.Add(deliveryLocation);
                     return deliveryOrderEntry;
                 }, new { id });

                var result = deliveryOrders.Values.ToList();

                if (result.Count == 0)
                    throw new KeyNotFoundException();

                return result;
            }
        }

        public async Task<DeliveryOrderViewModel> GetDeliveryOrderByIdAsync(Guid orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var multiResult = await connection.QueryMultipleAsync(SelectOrderByIdSqlQuery, new { orderId });

                var deliveryOrderViewModel = await multiResult.ReadSingleAsync<DeliveryOrderViewModel>();
                deliveryOrderViewModel.DeliveryLocations.AddRange(await multiResult.ReadAsync<DeliveryLocationViewModel>());
                return deliveryOrderViewModel;
            }
        }

        DeliveryOrderViewModel MapToDeliveryOrderViewModel(dynamic queryResult)
        {
            var order = new DeliveryOrderViewModel
            {
                Number = queryResult[0].Number,
                Weight = queryResult[0].Weight,
                CreatedDateTime = queryResult[0].CreatedDateTime,
                FinishedDateTime = queryResult[0].FinishedDateTime,
                PaymentAmount = queryResult[0].PaymentAmount,
                InsuranceAmount = queryResult[0].InsuranceAmount,
                Note = queryResult[0].Note,
                ShouldNotifySenderOnOrderStatusChange = queryResult[0].ShouldNotifySenderOnOrderStatusChange,
                ShouldNotifyRecipientOnOrderStatusChange = queryResult[0].ShouldNotifyRecipientOnOrderStatusChange,
                DeliveryOrderStatus = queryResult[0].DeliveryOrderStatus,
                ClientId = queryResult[0].ClientId,
                CourierId = queryResult[0].CourierId,
                DeliveryLocations = new List<DeliveryLocationViewModel>()
            };

            foreach (dynamic location in queryResult)
            {
                var orderitem = new DeliveryLocationViewModel
                {
                    Address = location.Address,
                    BuildingNumber = location.BuildingNumber,
                    EnterenceNumber = location.EnterenceNumber,
                    FloorNumber = location.FloorNumber,
                    ApartmentNumber = location.ApartmentNumber,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Note = location.Note,
                    BuyoutAmount = location.BuyoutAmount,
                    TakingAmount = location.TakingAmount,
                    IsPaymentInThisDeliveryLocation = location.IsPaymentInThisDeliveryLocation,
                    DeliveryLocationAction = location.DeliveryLocationAction,
                    ContactPersonName = location.ContactPersonName,
                    ContactPersonPhone = location.ContactPersonPhone,
                    ArrivalStartDateTime = location.ArrivalStartDateTime,
                    ArrivalFinishDateTime = location.ArrivalFinishDateTime,
                    CourierArrivedDateTime = location.CourierArrivedDateTime
                };
                order.DeliveryLocations.Add(orderitem);
            }
            return order;
        }
    }
}
