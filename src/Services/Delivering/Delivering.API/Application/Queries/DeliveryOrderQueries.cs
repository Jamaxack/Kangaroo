using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivering.API.Application.Queries
{
    public class DeliveryOrderQueries : IDeliveryOrderQueries
    {
        #region SqlQueries
        const string SelectDeliveryOrdersQuery =
              @"SELECT deliveryOrder.Id,
                       Number,
                       CreatedDateTime,
                       FinishedDateTime,
                       Price,
                       Weight,
                       Note,
                       PickUpLocation_Address,
                       PickUpLocation_BuildingNumber,
                       PickUpLocation_EntranceNumber,
                       PickUpLocation_FloorNumber,
                       PickUpLocation_ApartmentNumber,
                       PickUpLocation_Latitude,
                       PickUpLocation_Longitude,
                       PickUpLocation_Note,
                       PickUpLocation_ContactPerson_Name,
                       PickUpLocation_ContactPerson_Phone,
                       PickUpLocation_ArrivalStartDateTime,
                       PickUpLocation_ArrivalFinishDateTime,
                       PickUpLocation_CourierArrivedDateTime,
                       DropOffLocation_Address,
                       DropOffLocation_BuildingNumber,
                       DropOffLocation_EntranceNumber,
                       DropOffLocation_FloorNumber,
                       DropOffLocation_ApartmentNumber,
                       DropOffLocation_Latitude,
                       DropOffLocation_Longitude,
                       DropOffLocation_Note,
                       DropOffLocation_ContactPerson_Name,
                       DropOffLocation_ContactPerson_Phone,
                       DropOffLocation_ArrivalStartDateTime,
                       DropOffLocation_ArrivalFinishDateTime,
                       DropOffLocation_CourierArrivedDateTime,
                       ClientId,
                       CourierId,
	                   deliveryOrderStatus.Name as DeliveryOrderStatus
                  FROM Delivering.DeliveryOrders deliveryOrder
                  LEFT JOIN Delivering.DeliveryOrderStatus deliveryOrderStatus ON deliveryOrderStatus.Id = deliveryOrder.DeliveryOrderStatusId";
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
                var pagedQuery = $"{SelectDeliveryOrdersQuery} {@"Order BY deliveryOrder.Number OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY"}";
                var queryResult = await connection.QueryAsync(pagedQuery, param: new { offset, pageSize });

                var deliveryOrders = new List<DeliveryOrderViewModel>();
                foreach (var deliveryOrder in queryResult)
                    deliveryOrders.Add(MapToDeliveryOrderViewModel(deliveryOrder));

                return deliveryOrders;
            }
        }

        public async Task<List<DeliveryOrderViewModel>> GetDeliveryOrdersByClientIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryResult = await connection.QueryAsync($"{SelectDeliveryOrdersQuery} WHERE deliveryOrder.ClientId=@id", new { id });

                if (queryResult.Count() == 0)
                    throw new KeyNotFoundException();

                var deliveryOrders = new List<DeliveryOrderViewModel>();
                foreach (var deliveryOrder in queryResult)
                    deliveryOrders.Add(MapToDeliveryOrderViewModel(deliveryOrder));

                return deliveryOrders;
            }
        }

        public async Task<List<DeliveryOrderViewModel>> GetDeliveryOrdersByCourierIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryResult = await connection.QueryAsync($"{SelectDeliveryOrdersQuery} WHERE deliveryOrder.CourierId=@id", new { id });

                if (queryResult.Count() == 0)
                    throw new KeyNotFoundException();

                var deliveryOrders = new List<DeliveryOrderViewModel>();
                foreach (var deliveryOrder in queryResult)
                    deliveryOrders.Add(MapToDeliveryOrderViewModel(deliveryOrder));

                return deliveryOrders;
            }
        }

        public async Task<DeliveryOrderViewModel> GetDeliveryOrderByIdAsync(Guid deliveryOrderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = $"{SelectDeliveryOrdersQuery} WHERE deliveryOrder.Id = @deliveryOrderId;";
                var deliveryOrderDynamic = await connection.QueryFirstOrDefaultAsync(queryString, new { deliveryOrderId });

                if (deliveryOrderDynamic == null)
                    throw new KeyNotFoundException();

                return MapToDeliveryOrderViewModel(deliveryOrderDynamic);
            }
        }

        DeliveryOrderViewModel MapToDeliveryOrderViewModel(dynamic queryResult)
        {
            var pickUpContactPerson = new ContactPersonViewModel()
            {
                Name = queryResult.PickUpLocation_ContactPerson_Name,
                Phone = queryResult.PickUpLocation_ContactPerson_Phone
            };

            var pickUpDeliveryLocation = new DeliveryLocationViewModel
            {
                Address = queryResult.PickUpLocation_Address,
                BuildingNumber = queryResult.PickUpLocation_BuildingNumber,
                EntranceNumber = queryResult.PickUpLocation_EntranceNumber,
                FloorNumber = queryResult.PickUpLocation_FloorNumber,
                ApartmentNumber = queryResult.PickUpLocation_ApartmentNumber,
                Latitude = queryResult.PickUpLocation_Latitude,
                Longitude = queryResult.PickUpLocation_Longitude,
                Note = queryResult.PickUpLocation_Note,
                ContactPerson = pickUpContactPerson,
                ArrivalStartDateTime = queryResult.PickUpLocation_ArrivalStartDateTime,
                ArrivalFinishDateTime = queryResult.PickUpLocation_ArrivalFinishDateTime,
                CourierArrivedDateTime = queryResult.PickUpLocation_CourierArrivedDateTime
            };

            var dropOffContactPerson = new ContactPersonViewModel()
            {
                Name = queryResult.DropOffLocation_ContactPerson_Name,
                Phone = queryResult.DropOffLocation_ContactPerson_Phone
            };

            var dropOffDeliveryLocation = new DeliveryLocationViewModel
            {
                Address = queryResult.DropOffLocation_Address,
                BuildingNumber = queryResult.DropOffLocation_BuildingNumber,
                EntranceNumber = queryResult.DropOffLocation_EntranceNumber,
                FloorNumber = queryResult.DropOffLocation_FloorNumber,
                ApartmentNumber = queryResult.DropOffLocation_ApartmentNumber,
                Latitude = queryResult.DropOffLocation_Latitude,
                Longitude = queryResult.DropOffLocation_Longitude,
                Note = queryResult.DropOffLocation_Note,
                ContactPerson = dropOffContactPerson,
                ArrivalStartDateTime = queryResult.DropOffLocation_ArrivalStartDateTime,
                ArrivalFinishDateTime = queryResult.DropOffLocation_ArrivalFinishDateTime,
                CourierArrivedDateTime = queryResult.DropOffLocation_CourierArrivedDateTime
            };

            var deliveryOrder = new DeliveryOrderViewModel
            {
                Id = queryResult.Id,
                Number = queryResult.Number,
                CreatedDateTime = queryResult.CreatedDateTime,
                FinishedDateTime = queryResult.FinishedDateTime,
                Price = queryResult.Price,
                Weight = queryResult.Weight,
                Note = queryResult.Note,
                PickUpLocation = pickUpDeliveryLocation,
                DropOffLocation = dropOffDeliveryLocation,
                DeliveryOrderStatus = queryResult.DeliveryOrderStatus,
                ClientId = queryResult.ClientId,
                CourierId = queryResult.CourierId,
            };

            return deliveryOrder;
        }
    }
}
