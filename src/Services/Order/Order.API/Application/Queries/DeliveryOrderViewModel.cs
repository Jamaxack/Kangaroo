using System.Collections.Generic;

namespace Order.API.Application.Queries
{
    public class DeliveryOrder
    {
        public long number { get; set; }
        public string deliveryOrderStatus { get;   set; } 
        public List<DeliveryLocation> deliveryLocations { get; set; }

    }

    public class DeliveryLocation
    {
        public string address { get; set; }
        public string note { get; set; }
    }
}
