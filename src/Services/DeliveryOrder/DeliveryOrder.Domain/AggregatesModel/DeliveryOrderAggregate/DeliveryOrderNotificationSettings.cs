using DeliveryOrder.Domain.Common;
using System.Collections.Generic;

namespace DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate
{
    public class DeliveryOrderNotificationSettings : ValueObject
    {
        public bool ShouldNotifySenderOnDeliveryOrderStatusChange { get; private set; }
        public bool ShouldNotifyRecipientOnDeliveryOrderStatusChange { get; private set; }

        public DeliveryOrderNotificationSettings() { }

        public DeliveryOrderNotificationSettings(bool shouldNotifySenderOnDeliveryOrderStatusChange, bool shouldNotifyRecipientOnDeliveryOrderStatusChange)
        {
            ShouldNotifySenderOnDeliveryOrderStatusChange = shouldNotifySenderOnDeliveryOrderStatusChange;
            ShouldNotifyRecipientOnDeliveryOrderStatusChange = shouldNotifyRecipientOnDeliveryOrderStatusChange;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ShouldNotifySenderOnDeliveryOrderStatusChange;
            yield return ShouldNotifyRecipientOnDeliveryOrderStatusChange;
        }
    }
}
