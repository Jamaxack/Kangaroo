using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Courier.API.Model
{
    public class CourierLocation
    {
        [BsonId]
        public Guid Id { get; set; }
        public Guid CourierId { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public DateTime DateTime { get; set; }
    }
}
