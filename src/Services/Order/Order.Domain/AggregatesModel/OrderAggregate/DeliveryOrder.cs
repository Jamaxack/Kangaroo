using Order.Domain.AggregatesModel.CourierAggregate;
using Order.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class DeliveryOrder : Entity, IAggregateRoot
    {
        public long Number { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? FinishedDateTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public DeliveryOrderNotificationSettings DeliveryOrderNotificationSettings { get; set; } 
        public decimal PaymentAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string Note { get; set; }
        public int Weight { get; set; }
        public List<Point> Points { get; set; }
        public Guid? CourierId { get; set; }
        public Guid ClientId { get; set; }
        List<OrderStatus> OrderStatusHistory { get; set; }
        List<DeliveryStatus> DeliveryStatusHistory { get; set; }
    }
}
