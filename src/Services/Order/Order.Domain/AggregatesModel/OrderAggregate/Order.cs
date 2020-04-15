using Order.Domain.AggregatesModel.CourierAggregate;
using Order.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public long Number { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? FinishedDateTime { get; set; }
        public OrderStatus Status { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string Note { get; set; }
        public int Weight { get; set; }
        public bool ShouldNotifySenderOnOrderStatusChange { get; set; }
        public bool ShouldNotifyRecipientOnOrderStatusChange { get; set; }
        public List<Point> Points { get; set; }
        public Courier Courier { get; set; }
    }
}
