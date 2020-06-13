using System;

namespace Pricing.API.DataTransferableObjects
{
    public class DeliveryLocationDto
    {
        public string Address { get; set; }
        public DateTime? ArrivalStartDateTime { get; set; }
        public DateTime? ArrivalFinishDateTime { get; set; }
    }
}
