using System;

namespace Courier.API.DataTransferableObjects
{
    public class CourierLocationDto
    {
        public Guid CourierId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}