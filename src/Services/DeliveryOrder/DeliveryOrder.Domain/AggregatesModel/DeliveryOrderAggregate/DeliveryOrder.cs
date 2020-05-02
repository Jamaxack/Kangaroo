using DeliveryOrder.Domain.Common;
using DeliveryOrder.Domain.Events;
using System;
using System.Collections.Generic;

namespace DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate
{
    public class DeliveryOrder : Entity, IAggregateRoot
    {
        public long Number { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime? FinishedDateTime { get; private set; }
        public decimal Price { get; private set; }
        public short Weight { get; private set; }
        public string Note { get; private set; }

        int _deliveryOrderStatusId;
        public DeliveryOrderStatus DeliveryOrderStatus { get; private set; }

        Guid _clientId;
        public Guid GetClientId => _clientId;

        Guid? _courierId;
        public Guid? GetCourierId => _courierId;

        readonly List<DeliveryLocation> _deliveryLocations;
        public IReadOnlyCollection<DeliveryLocation> DeliveryLocations => _deliveryLocations;

        public DeliveryOrder()
        {
            _deliveryLocations = new List<DeliveryLocation>();
            CreatedDateTime = DateTime.UtcNow;
            _deliveryOrderStatusId = DeliveryOrderStatus.New.Id;
        }

        public DeliveryOrder(Guid clientId, decimal price, short weight, string note) : this()
        {
            _clientId = clientId;
            Price = price;
            Weight = weight;
            Note = note;

            AddDomainEvent(new DeliveryOrderCreatedDomainEvent(this, clientId));
        }

        public void AddDelivaryLocation(DeliveryLocation deliveryLocation)
        {
            _deliveryLocations.Add(deliveryLocation);
        }

        public void RemoveDeliveryLocation(DeliveryLocation deliveryLocation)
        {
            _deliveryLocations.Remove(deliveryLocation);
        }

        public void SetDeliveryOrderAvailableStatus()
        {
            var deliveryOrderStatusBeforeChange = _deliveryOrderStatusId;
            _deliveryOrderStatusId = DeliveryOrderStatus.Available.Id;
            AddDomainEvent(new DeliveryOrderStatusChangedToAvailableDomainEvent(Id, deliveryOrderStatusBeforeChange));
        }

        public void SetCourierId(Guid id)
        {
            _courierId = id;
            _deliveryOrderStatusId = DeliveryOrderStatus.CourierAssigned.Id;
        }
    }
}
