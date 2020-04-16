using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class DeliveryOrderNotificationSettings
    {
        public bool ShouldNotifySenderOnOrderStatusChange { get; set; }
        public bool ShouldNotifyRecipientOnOrderStatusChange { get; set; }
    }
}
