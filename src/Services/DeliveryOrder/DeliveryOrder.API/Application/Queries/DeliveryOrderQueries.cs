using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryOrder.API.Application.Queries
{
    public class DeliveryOrderQueries : IDeliveryOrderQueries
    {
        #region SqlQueries
        const string SelectDeliveryOrdersQuery =
            @"SELECT 
	                do.[Id], 
	                do.[Number],
	                do.[CreatedDateTime],
	                do.[FinishedDateTime],
	                do.[Price], 
	                do.[Weight],
	                do.[Note],
	                do.[ClientId],
	                do.[CourierId],
	                dos.[Name] as DeliveryOrderStatus,
	                dla.[Name] as DeliveryLocationAction,
	                dl.[Id],
	                dl.[Address],
	                dl.[BuildingNumber],
	                dl.[EntranceNumber],
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
	                [DeliveryOrder].DeliveryOrders do
	                LEFT JOIN [DeliveryOrder].DeliveryLocations dl ON do.Id = dl.DeliveryOrderId
	                LEFT JOIN [DeliveryOrder].DeliveryOrderStatus dos ON do.DeliveryOrderStatusId = dos.Id
	                LEFT JOIN [DeliveryOrder].DeliveryLocationActions dla ON dl.DeliveryLocationActionId = dla.Id";

        const string SelectDeliveryOrderByIdSqlQuery =
            @"SELECT TOP(1)
                    do.[Id],
                       [Number],
                       [CreatedDateTime],
                       [FinishedDateTime],
                       [Price],
                       [Weight],
                       [Note],
                       [ClientId],
                       [CourierId],
	                   dos.[Name] as DeliveryOrderStatus
                  FROM [Kangaroo.Services.DeliveryOrder].[DeliveryOrder].[DeliveryOrders] as do
                  LEFT JOIN [DeliveryOrder].DeliveryOrderStatus dos ON dos.Id = do.DeliveryOrderStatusId
                  WHERE do.[Id] = @deliveryOrderId;
   
                SELECT
                      dl.[Id],
                      [Address],
                      [BuildingNumber],
                      [EntranceNumber],
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
                  FROM [DeliveryOrder].[DeliveryLocations] as dl
                  LEFT JOIN [DeliveryOrder].DeliveryLocationActions dla ON dla.Id = dl.DeliveryLocationActionId 
                  WHERE dl.[DeliveryOrderId] = @deliveryOrderId";
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
                var pagedQuery = $"{SelectDeliveryOrdersQuery} {@"Order BY do.[Number] OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY"}";

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
                 $"{SelectDeliveryOrdersQuery} WHERE do.ClientId=@id",
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
               $"{SelectDeliveryOrdersQuery} WHERE do.CourierId=@id",
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

        public async Task<DeliveryOrderViewModel> GetDeliveryOrderByIdAsync(Guid deliveryOrderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var multiResult = await connection.QueryMultipleAsync(SelectDeliveryOrderByIdSqlQuery, new { deliveryOrderId });

                var deliveryOrderViewModel = await multiResult.ReadSingleAsync<DeliveryOrderViewModel>();
                deliveryOrderViewModel.DeliveryLocations.AddRange(await multiResult.ReadAsync<DeliveryLocationViewModel>());
                return deliveryOrderViewModel;
            }
        }

        DeliveryOrderViewModel MapToDeliveryOrderViewModel(dynamic queryResult)
        {
            var deliveryOrder = new DeliveryOrderViewModel
            {
                Number = queryResult[0].Number,
                Weight = queryResult[0].Weight,
                CreatedDateTime = queryResult[0].CreatedDateTime,
                FinishedDateTime = queryResult[0].FinishedDateTime,
                Price = queryResult[0].Price, 
                Note = queryResult[0].Note, 
                DeliveryOrderStatus = queryResult[0].DeliveryOrderStatus,
                ClientId = queryResult[0].ClientId,
                CourierId = queryResult[0].CourierId,
                DeliveryLocations = new List<DeliveryLocationViewModel>()
            };

            foreach (dynamic location in queryResult)
            {
                var deliveryLocation = new DeliveryLocationViewModel
                {
                    Address = location.Address,
                    BuildingNumber = location.BuildingNumber,
                    EntranceNumber = location.EntranceNumber,
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
                deliveryOrder.DeliveryLocations.Add(deliveryLocation);
            }
            return deliveryOrder;
        }
    }
}
