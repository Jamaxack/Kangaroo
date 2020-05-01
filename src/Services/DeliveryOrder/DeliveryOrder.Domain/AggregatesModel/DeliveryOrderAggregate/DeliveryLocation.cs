using DeliveryOrder.Domain.AggregatesModel.ClientAggregate;
using DeliveryOrder.Domain.Common;
using System;
using System.Collections.Generic;

namespace DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate
{
    public class DeliveryLocation : Entity
    {
        int _deliveryLocationActionId;

        public string Address { get; private set; }
        public string BuildingNumber { get; private set; }
        public string EntranceNumber { get; private set; }
        public string FloorNumber { get; private set; }
        public string ApartmentNumber { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Note { get; private set; } // Navigation instruction, etc.
        public decimal BuyoutAmount { get; private set; } //Amount that courier bought something for client
        public decimal TakingAmount { get; private set; } //Amount that client should pay for courier: Delivery price + Buyout price
        public bool IsPaymentInThisDeliveryLocation { get; private set; } //Determines if payment will be in this DeliveryLocation
        public DeliveryLocationAction DeliveryLocationAction { get; private set; } // PickUp or DropOff
        public ContactPerson ContactPerson { get; private set; }//Sender or Recipient 
        public DateTime? ArrivalStartDateTime { get; private set; }
        public DateTime? ArrivalFinishDateTime { get; private set; }
        public DateTime? CourierArrivedDateTime { get; private set; }

        public DeliveryLocation() { }

        public DeliveryLocation(string address, string buildingNumber, string EntranceNumber, string floorNumber, string apartmentNumber, double latitude,
            double longitude, string note, decimal buyoutAmount, decimal takingAmount, bool isPaymentInThisDeliveryLocation, int deliveryLocationtActionId,
            DateTime? arrivalStartDateTime, DateTime? arrivalFinishDateTime, DateTime? courierArrivedDateTime, ContactPerson contactPerson)
        {
            Address = address;
            BuildingNumber = buildingNumber;
            EntranceNumber = EntranceNumber;
            FloorNumber = floorNumber;
            ApartmentNumber = apartmentNumber;
            Latitude = latitude;
            Longitude = longitude;
            Note = note;
            BuyoutAmount = buyoutAmount;
            TakingAmount = takingAmount;
            ArrivalStartDateTime = arrivalStartDateTime;
            ArrivalFinishDateTime = arrivalFinishDateTime;
            CourierArrivedDateTime = courierArrivedDateTime;
            ContactPerson = contactPerson;
            _deliveryLocationActionId = deliveryLocationtActionId;
            IsPaymentInThisDeliveryLocation = isPaymentInThisDeliveryLocation;
        }
    }
}
