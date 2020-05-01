using DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;

namespace DeliveryOrder.API.Application.Commands
{
    public class DeliveryLocationDTO
    {
        public string Address { get; set; }
        public string BuildingNumber { get; set; }
        public string EntranceNumber { get; set; }
        public string FloorNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Note { get; set; } // Navigation instruction, etc.
        public decimal BuyoutAmount { get; set; } //Amount that courier bought something for client
        public decimal TakingAmount { get; set; } //Amount that client should pay for courier: Delivery price + Buyout price
        public bool IsPaymentInThisDeliveryLocation { get; set; } //Determines if payment will be in this DeliveryLocation
        public int DeliveryLocationActionId { get; set; } // PickUp or DropOff
        public string ContactPersonName { get; set; }//Sender or Recipient 
        public string ContactPersonPhone { get; set; }//Sender or Recipient 
        public DateTime? ArrivalStartDateTime { get; set; }
        public DateTime? ArrivalFinishDateTime { get; set; }
        public DateTime? CourierArrivedDateTime { get; set; }

        public static DeliveryLocationDTO FromLocation(DeliveryLocation location)
        {
            return new DeliveryLocationDTO()
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
                DeliveryLocationActionId = location.DeliveryLocationAction.Id,
                ContactPersonName = location.ContactPerson.Name,
                ContactPersonPhone = location.ContactPerson.Phone,
                ArrivalStartDateTime = location.ArrivalStartDateTime,
                ArrivalFinishDateTime = location.ArrivalFinishDateTime,
                CourierArrivedDateTime = location.CourierArrivedDateTime
            };
        }
    }
}
