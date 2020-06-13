using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Courier.API.Model
{
    public class CourierLocation
    {
        [BsonId] public Guid Id { get; set; }

        public Guid CourierId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateTime { get; set; }
    }
}