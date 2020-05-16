using Delivery.Domain.AggregatesModel.ClientAggregate;
using Delivery.Domain.Common;
using System;
using System.Collections.Generic;

namespace Delivery.Domain.AggregatesModel.DeliveryAggregate
{
    public class DeliveryLocation : ValueObject
    {
        public string Address { get; private set; }
        public string BuildingNumber { get; private set; }
        public string EntranceNumber { get; private set; }
        public string FloorNumber { get; private set; }
        public string ApartmentNumber { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Note { get; private set; } // Navigation instruction, etc.
        // ContactPerson is a Value Object pattern example persisted as EF Core 2.0 owned entity
        public ContactPerson ContactPerson { get; private set; }//Sender or Recipient 
        public DateTime? ArrivalStartDateTime { get; private set; }
        public DateTime? ArrivalFinishDateTime { get; private set; }
        public DateTime? CourierArrivedDateTime { get; private set; }

        public DeliveryLocation() { }

        public DeliveryLocation(string address, string buildingNumber, string entranceNumber, string floorNumber, string apartmentNumber, double latitude, double longitude, string note, DateTime? arrivalStartDateTime, DateTime? arrivalFinishDateTime, DateTime? courierArrivedDateTime, ContactPerson contactPerson)
        {
            Address = address;
            BuildingNumber = buildingNumber;
            EntranceNumber = entranceNumber;
            FloorNumber = floorNumber;
            ApartmentNumber = apartmentNumber;
            Latitude = latitude;
            Longitude = longitude;
            Note = note;
            ArrivalStartDateTime = arrivalStartDateTime;
            ArrivalFinishDateTime = arrivalFinishDateTime;
            CourierArrivedDateTime = courierArrivedDateTime;
            ContactPerson = contactPerson;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Address;
            yield return BuildingNumber;
            yield return EntranceNumber;
            yield return FloorNumber;
            yield return ApartmentNumber;
            yield return Latitude;
            yield return Longitude;
            yield return Note;
        }
    }
}
