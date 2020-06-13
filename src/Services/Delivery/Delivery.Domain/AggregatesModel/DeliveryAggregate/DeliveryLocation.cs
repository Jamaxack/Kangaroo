using System;
using System.Collections.Generic;
using Delivery.Domain.Common;

namespace Delivery.Domain.AggregatesModel.DeliveryAggregate
{
    public class DeliveryLocation : ValueObject
    {
        public DeliveryLocation()
        {
        }

        public DeliveryLocation(string address, string buildingNumber, string entranceNumber, string floorNumber,
            string apartmentNumber, double latitude, double longitude, string note, DateTime? arrivalStartDateTime,
            DateTime? arrivalFinishDateTime, DateTime? courierArrivedDateTime, ContactPerson contactPerson)
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

        public string Address { get; }
        public string BuildingNumber { get; }
        public string EntranceNumber { get; }
        public string FloorNumber { get; }
        public string ApartmentNumber { get; }
        public double Latitude { get; }
        public double Longitude { get; }

        public string Note { get; } // Navigation instruction, etc.

        // ContactPerson is a Value Object pattern example persisted as EF Core 2.0 owned entity
        public ContactPerson ContactPerson { get; } //Sender or Recipient 
        public DateTime? ArrivalStartDateTime { get; }
        public DateTime? ArrivalFinishDateTime { get; }
        public DateTime? CourierArrivedDateTime { get; }

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