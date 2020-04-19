using Order.Domain.Common;
using System.Collections.Generic;

namespace Order.Domain.AggregatesModel.DeliveryOrderAggregate
{
    public class DeliveryOrderNotificationSettings : ValueObject
    {
        public bool ShouldNotifySenderOnOrderStatusChange { get; private set; }
        public bool ShouldNotifyRecipientOnOrderStatusChange { get; private set; }

        public DeliveryOrderNotificationSettings() { }

        public DeliveryOrderNotificationSettings(bool shouldNotifySenderOnOrderStatusChange, bool shouldNotifyRecipientOnOrderStatusChange)
        {
            ShouldNotifySenderOnOrderStatusChange = shouldNotifySenderOnOrderStatusChange;
            ShouldNotifyRecipientOnOrderStatusChange = shouldNotifyRecipientOnOrderStatusChange;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ShouldNotifySenderOnOrderStatusChange;
            yield return ShouldNotifyRecipientOnOrderStatusChange;
        }
    }
}
