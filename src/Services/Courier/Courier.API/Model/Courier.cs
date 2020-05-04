using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Courier.API.Model
{
    public class Courier
    {
        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}
