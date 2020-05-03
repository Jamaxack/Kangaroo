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
        public ContactPersonDTO ContactPerson { get; set; }//Sender or Recipient 
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
                ContactPerson = ContactPersonDTO.FromContactPerson(location.ContactPerson),
                ArrivalStartDateTime = location.ArrivalStartDateTime,
                ArrivalFinishDateTime = location.ArrivalFinishDateTime,
                CourierArrivedDateTime = location.CourierArrivedDateTime
            };
        }

        public DeliveryLocation GetDeliveryLocation()
        {
            var contactPerson = new ContactPerson(ContactPerson.Name, ContactPerson.Phone);
            return new DeliveryLocation(Address, BuildingNumber, EntranceNumber, FloorNumber, ApartmentNumber, Latitude, Longitude, Note, ArrivalStartDateTime, ArrivalFinishDateTime, CourierArrivedDateTime, contactPerson);
        }
    }
}
