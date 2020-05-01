using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryOrder.API
{
    public class DeliveryOrderSettings
    {
        public bool UseCustomizationData { get; set; }

        public string ConnectionString { get; set; }
    }
}
