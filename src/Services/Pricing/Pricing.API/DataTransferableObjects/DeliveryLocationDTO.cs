using System;

namespace Pricing.API.DataTransferableObjects
{
    public class DeliveryLocationDTO
    {
        public string Address { get; set; }
        public DateTime? ArrivalStartDateTime { get; set; }
        public DateTime? ArrivalFinishDateTime { get; set; }
    }
}
