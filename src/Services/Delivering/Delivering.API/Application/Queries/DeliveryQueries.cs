using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivering.API.Application.Queries
{
    public class DeliveryQueries : IDeliveryQueries
    {
        #region SqlQueries
        const string SelectDeliverysQuery =
              @"SELECT Delivery.Id,
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
	                   DeliveryStatus.Name as DeliveryStatus
                  FROM Delivering.Deliveries Delivery
                  LEFT JOIN Delivering.DeliveryStatus DeliveryStatus ON DeliveryStatus.Id = Delivery.DeliveryStatusId";
        #endregion

        string _connectionString = string.Empty;
        public DeliveryQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<List<DeliveryViewModel>> GetDeliverysAsync(int pageSize, int pageIndex)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var offset = pageSize * pageIndex;
                var pagedQuery = $"{SelectDeliverysQuery} {@"Order BY Delivery.Number OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY"}";
                var queryResult = await connection.QueryAsync(pagedQuery, param: new { offset, pageSize });

                var deliveries = new List<DeliveryViewModel>();
                foreach (var Delivery in queryResult)
                    deliveries.Add(MapToDeliveryViewModel(Delivery));

                return deliveries;
            }
        }

        public async Task<List<DeliveryViewModel>> GetDeliverysByClientIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryResult = await connection.QueryAsync($"{SelectDeliverysQuery} WHERE Delivery.ClientId=@id", new { id });

                if (queryResult.Count() == 0)
                    throw new KeyNotFoundException();

                var deliveries = new List<DeliveryViewModel>();
                foreach (var Delivery in queryResult)
                    deliveries.Add(MapToDeliveryViewModel(Delivery));

                return deliveries;
            }
        }

        public async Task<List<DeliveryViewModel>> GetDeliverysByCourierIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryResult = await connection.QueryAsync($"{SelectDeliverysQuery} WHERE Delivery.CourierId=@id", new { id });

                if (queryResult.Count() == 0)
                    throw new KeyNotFoundException();

                var deliveries = new List<DeliveryViewModel>();
                foreach (var Delivery in queryResult)
                    deliveries.Add(MapToDeliveryViewModel(Delivery));

                return deliveries;
            }
        }

        public async Task<DeliveryViewModel> GetDeliveryByIdAsync(Guid DeliveryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = $"{SelectDeliverysQuery} WHERE Delivery.Id = @DeliveryId;";
                var DeliveryDynamic = await connection.QueryFirstOrDefaultAsync(queryString, new { DeliveryId });

                if (DeliveryDynamic == null)
                    throw new KeyNotFoundException();

                return MapToDeliveryViewModel(DeliveryDynamic);
            }
        }

        DeliveryViewModel MapToDeliveryViewModel(dynamic queryResult)
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

            var Delivery = new DeliveryViewModel
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
                DeliveryStatus = queryResult.DeliveryStatus,
                ClientId = queryResult.ClientId,
                CourierId = queryResult.CourierId,
            };

            return Delivery;
        }
    }
}
