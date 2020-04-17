using Order.Domain.AggregatesModel.CourierAggregate;
using Order.Domain.Common;
using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class DeliveryOrder : Entity, IAggregateRoot
    {
        public long Number { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime? FinishedDateTime { get; private set; }
        public decimal PaymentAmount { get; private set; }
        public decimal InsuranceAmount { get; private set; }
        public short Weight { get; private set; }
        public string Note { get; private set; }

        short _orderStatusId;
        public OrderStatus OrderStatus => OrderStatus.From(_orderStatusId);
        
        short _deliveryStatusId;
        public DeliveryStatus DeliveryStatus => DeliveryStatus.From(_deliveryStatusId);

        // DeliveryOrderNotificationSettings is a Value Object pattern example persisted as EF Core 2.0 owned entity
        public DeliveryOrderNotificationSettings DeliveryOrderNotificationSettings { get; private set; }

        Guid _clientId;
        public Guid GetClientId => _clientId;

        Guid? _courierId;
        public Guid? GetCourierId => _courierId;

        readonly List<Point> _points;
        public IReadOnlyCollection<Point> Points => _points;

        readonly List<OrderStatus> _orderStatusHistory;
        public IReadOnlyCollection<OrderStatus> OrderStatusHistory => _orderStatusHistory;

        readonly List<DeliveryStatus> _deliveryStatusHistory;
        public IReadOnlyCollection<DeliveryStatus> DeliveryStatusHistory => _deliveryStatusHistory;

        public DeliveryOrder()
        {
            _points = new List<Point>();
            _orderStatusHistory = new List<OrderStatus>();
            _deliveryStatusHistory = new List<DeliveryStatus>();
            CreatedDateTime = DateTime.UtcNow;
            _orderStatusId = OrderStatus.New.Id;
        }

        public DeliveryOrder(Guid clientId, DeliveryOrderNotificationSettings deliveryOrderNotificationSettings, decimal paymentAmount, decimal insuranceAmount, short weight, string note) : this()
        {
            _clientId = clientId;
            DeliveryOrderNotificationSettings = deliveryOrderNotificationSettings;
            PaymentAmount = paymentAmount;
            InsuranceAmount = insuranceAmount;
            Weight = weight;
            Note = note;
        }

        public void SetOrderStatusAvailable()
        {

        }

        public void SetCourierId(Guid id)
        {

        }


        public void AddDelivaryPoint()
        {

        }

        public void SetOrderStatus()
        {

        }

        public void SetDeliveryStatus()
        {

        }

        private void StatusChangeException(OrderStatus orderStatusToChange)
        {
            throw new OrderingDomainException($"Is not possible to change the order status from {OrderStatus.Name} to {orderStatusToChange.Name}.");
        }
    }
}
