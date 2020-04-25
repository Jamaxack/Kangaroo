﻿using Dapper;
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

        public async Task<DeliveryOrderViewModel> GetDeliveryOrderByIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<dynamic>(
                  @"SELECT
                        do.Number, do.CreatedDateTime, do.FinishedDateTime, do.PaymentAmount, do.InsuranceAmount, do.Weight, do.Note, 
                        do.DeliveryOrderNotificationSettings_ShouldNotifySenderOnOrderStatusChange as ShouldNotifySenderOnOrderStatusChange,
                        do.DeliveryOrderNotificationSettings_ShouldNotifyRecipientOnOrderStatusChange as ShouldNotifyRecipientOnOrderStatusChange, 
                        do.ClientId, do.CourierId, dos.Name as DeliveryOrderStatus, dla.Name as DeliveryLocationAction, dl.Address,
                        dl.BuildingNumber, dl.EnterenceNumber, dl.FloorNumber, dl.ApartmentNumber, dl.Latitude, dl.Longitude, dl.Note,
                        dl.BuyoutAmount, dl.TakingAmount, dl.IsPaymentInThisDeliveryLocation, dl.ContactPerson_Name as ContactPersonName,
                        dl.ContactPerson_Phone as ContactPersonPhone, dl.ArrivalStartDateTime, dl.ArrivalFinishDateTime, dl.CourierArrivedDateTime
                    FROM
                        [Order].DeliveryOrders do
                        LEFT JOIN [Order].DeliveryLocations dl ON do.Id = dl.DeliveryOrderId 
                        LEFT JOIN [Order].DeliveryOrderStatus dos ON do.DeliveryOrderStatusId = dos.Id
                        LEFT JOIN [Order].DeliveryLocationActions dla ON dl.DeliveryLocationActionId = dla.Id
                    WHERE do.Id=@id", new { id });

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return MapToDeliveryOrderViewModel(result);
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