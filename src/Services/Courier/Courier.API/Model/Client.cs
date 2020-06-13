using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Courier.API.Model
{
    public class Client
    {
        [BsonId] public Guid Id { get; set; }

        public string Phone { get; set; }
        public string FullName { get; set; }
    }
}