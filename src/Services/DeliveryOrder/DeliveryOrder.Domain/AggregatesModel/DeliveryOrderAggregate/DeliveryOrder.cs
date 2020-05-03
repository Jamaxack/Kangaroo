using DeliveryOrder.Domain.Common;
using DeliveryOrder.Domain.Events;
using System;

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
        public DeliveryLocation PickUpLocation { get; private set; }
        public DeliveryLocation DropOffLocation { get; private set; }

        Guid _clientId;
        public Guid GetClientId => _clientId;

        Guid? _courierId;
        public Guid? GetCourierId => _courierId;

        public DeliveryOrder()
        {
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

        public void SetPickUpLocation(DeliveryLocation pickUpLocation)
        {
            PickUpLocation = pickUpLocation;
        }

        public void SetDropOffLocation(DeliveryLocation dropOffLocation)
        {
            DropOffLocation = dropOffLocation;
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
