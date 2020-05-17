using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Delivering.HttpAggregator.Models
{
    public class Delivery
    {
        public Guid DeliveryId { get; set; }
        public string Status { get; set; }
        public long Number { get; set; }
        public string Note { get; set; }
    }
}
