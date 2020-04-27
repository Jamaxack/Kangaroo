using Order.Domain.Common;
using Order.Domain.Events;
using System;
using System.Collections.Generic;

namespace Order.Domain.AggregatesModel.DeliveryOrderAggregate
{
    public class DeliveryOrder : Entity, IAggregateRoot
    {
        int _deliveryOrderStatusId;
        Guid _clientId;
        Guid? _courierId;
        readonly List<DeliveryLocation> _deliveryLocations;

        public long Number { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime? FinishedDateTime { get; private set; }
        public decimal PaymentAmount { get; private set; }
        public decimal InsuranceAmount { get; private set; }
        public short Weight { get; private set; }
        public string Note { get; private set; }
        // DeliveryOrderNotificationSettings is a Value Object pattern example persisted as EF Core 2.0 owned entity
        public DeliveryOrderNotificationSettings DeliveryOrderNotificationSettings { get; private set; }
        public DeliveryOrderStatus DeliveryOrderStatus { get; private set; }
        public Guid GetClientId => _clientId;
        public Guid? GetCourierId => _courierId;
        public IReadOnlyCollection<DeliveryLocation> DeliveryLocations => _deliveryLocations;

        public DeliveryOrder()
        {
            _deliveryLocations = new List<DeliveryLocation>();
            CreatedDateTime = DateTime.UtcNow;
            _deliveryOrderStatusId = DeliveryOrderStatus.New.Id;
        }

        public DeliveryOrder(Guid clientId, decimal paymentAmount, decimal insuranceAmount, short weight, string note,
            DeliveryOrderNotificationSettings deliveryOrderNotificationSettings) : this()
        {
            _clientId = clientId;
            DeliveryOrderNotificationSettings = deliveryOrderNotificationSettings;
            PaymentAmount = paymentAmount;
            InsuranceAmount = insuranceAmount;
            Weight = weight;
            Note = note;

            AddDomainEvent(new NewDeliveryOrderCreatedDomainEvent(this, clientId));
        }

        public void AddDelivaryLocation(DeliveryLocation deliveryLocation)
        { 
            _deliveryLocations.Add(deliveryLocation);
        }

        public void RemoveDeliveryLocation(DeliveryLocation deliveryLocation)
        {
            _deliveryLocations.Remove(deliveryLocation);
        }

        public void SetOrderAvailableStatus()
        {
            _deliveryOrderStatusId = DeliveryOrderStatus.Available.Id;
        }

        public void SetCourierId(Guid id)
        {
            _courierId = id;
            _deliveryOrderStatusId = DeliveryOrderStatus.CourierAssigned.Id;
        }
    }
}
