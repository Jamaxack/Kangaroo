using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Courier.API.Model
{
    public class Courier
    {
        [BsonId] public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public List<Delivery> Deliveries { get; set; }
    }
}