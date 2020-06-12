using System;
using Courier.API.Model;

namespace Courier.API.DataTransferableObjects
{
    public class DeliveryDtoSave
    { 
        public long Number { get; set; }
        public decimal Price { get; set; }
        public short Weight { get; set; }
        public string Note { get; set; }

        public DeliveryStatus DeliveryStatus { get; set; }
        public DeliveryLocationDto PickUpLocation { get; set; }
        public DeliveryLocationDto DropOffLocation { get; set; }

        public Guid CourierId { get; set; }
        public Guid ClientId { get; set; }
        public Guid DeliveryId { get; set; } //Reference to original Delivery from Delivery service
    }
}