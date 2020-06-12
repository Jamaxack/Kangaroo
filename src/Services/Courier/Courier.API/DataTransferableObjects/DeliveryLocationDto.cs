using System; 

namespace Courier.API.DataTransferableObjects
{
    public class DeliveryLocationDto
    {
        public string Address { get; set; }
        public string BuildingNumber { get; set; }
        public string EntranceNumber { get; set; }
        public string FloorNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Note { get; set; } // Navigation instruction, etc.
        public ContactPersonDto ContactPerson { get; set; }//Sender or Recipient 
        public DateTime? ArrivalStartDateTime { get; set; }
        public DateTime? ArrivalFinishDateTime { get; set; }
    }
}