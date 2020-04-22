using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API
{
    public class DeliveryOrderSettings
    {
        public bool UseCustomizationData { get; set; }

        public string ConnectionString { get; set; }
    }
}
