using Order.Domain.AggregatesModel.ClientAggregate;
using Order.Domain.Common;
using System;
using System.Collections.Generic;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class Point : ValueObject
    {
        public string Address { get; private set; }
        public string BuildingNumber { get; private set; }
        public string EnterenceNumber { get; private set; }
        public string FloorNumber { get; private set; }
        public string ApartmentNumber { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Note { get; private set; } // Navigation instruction, etc.
        public decimal BuyoutAmount { get; private set; } //Amount that courier bought something for client
        public decimal TakingAmount { get; private set; } //Amount that client should pay for courier: Delivery price + Buyout price
        public bool IsPaymentInThisPoint { get; private set; } //Determines if payment will be in this point

        short _actionId;
        public PointAction PointAction => PointAction.From(_actionId); // PickUp or DropOff

        public DateTime? ArrivalStartDateTime { get; private set; }
        public DateTime? ArrivalFinishDateTime { get; private set; }
        public DateTime? CourierArrivedDateTime { get; private set; }

        public ContactPerson ContactPerson { get; private set; }//Sender or Recipient 

        public Point() { }

        public Point(string address, string buildingNumber, string enterenceNumber, string floorNumber, string apartmentNumber, double latitude,
            double longitude, string note, decimal buyoutAmount, decimal takingAmount, bool isPaymentInThisPoint, short actionId,
            DateTime? arrivalStartDateTime, DateTime? arrivalFinishDateTime, DateTime? courierArrivedDateTime, ContactPerson contactPerson)
        {
            Address = address;
            BuildingNumber = buildingNumber;
            EnterenceNumber = enterenceNumber;
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
            _actionId = actionId;
            IsPaymentInThisPoint = isPaymentInThisPoint;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time 
            yield return Address;
            yield return BuildingNumber;
            yield return EnterenceNumber;
            yield return FloorNumber;
            yield return ApartmentNumber;
            yield return Latitude;
            yield return Longitude;
            yield return Note;
            yield return BuyoutAmount;
            yield return TakingAmount;
            yield return ArrivalStartDateTime;
            yield return ArrivalFinishDateTime;
            yield return CourierArrivedDateTime;
            yield return IsPaymentInThisPoint;
        }
    }
}
