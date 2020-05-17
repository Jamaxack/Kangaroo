using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Courier.API.Model
{
    public class Client
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
    }
}
