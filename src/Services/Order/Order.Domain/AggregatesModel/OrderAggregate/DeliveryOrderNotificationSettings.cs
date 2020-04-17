using Order.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class DeliveryOrderNotificationSettings : ValueObject
    {
        public bool ShouldNotifySenderOnOrderStatusChange { get; set; }
        public bool ShouldNotifyRecipientOnOrderStatusChange { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ShouldNotifySenderOnOrderStatusChange;
            yield return ShouldNotifyRecipientOnOrderStatusChange;
        }
    }
}
