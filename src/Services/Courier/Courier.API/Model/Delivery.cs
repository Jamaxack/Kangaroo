using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Courier.API.Model
{
    public class Delivery
    {
        [BsonId]
        public Guid Id { get; set; }

        public long Number { get; set; }
        public decimal Price { get; set; }
        public short Weight { get; set; }
        public string Note { get; set; }

        public DeliveryStatus DeliveryStatus { get; set; }
        public DeliveryLocation PickUpLocation { get; set; }
        public DeliveryLocation DropOffLocation { get; set; }

        public Guid CourierId { get; set; }
        public Guid ClientId { get; set; }
        public Guid DeliveryId { get; set; } //Reference to original Delivery from Delivery service
    }
}