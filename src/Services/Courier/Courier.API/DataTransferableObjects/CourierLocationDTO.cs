using System;

namespace Courier.API.DataTransferableObjects
{
    public class CourierLocationDTO
    {
        public Guid CourierId { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
}
