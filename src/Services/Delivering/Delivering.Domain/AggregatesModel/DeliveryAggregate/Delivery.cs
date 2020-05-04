using Delivering.Domain.Common;
using Delivering.Domain.Events;
using System;

namespace Delivering.Domain.AggregatesModel.DeliveryAggregate
{
    public class Delivery : Entity, IAggregateRoot
    {
        public long Number { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime? FinishedDateTime { get; private set; }
        public decimal Price { get; private set; }
        public short Weight { get; private set; }
        public string Note { get; private set; }

        int _DeliveryStatusId;
        public DeliveryStatus DeliveryStatus { get; private set; }
        public DeliveryLocation PickUpLocation { get; private set; }
        public DeliveryLocation DropOffLocation { get; private set; }

        Guid _clientId;
        public Guid GetClientId => _clientId;

        Guid? _courierId;
        public Guid? GetCourierId => _courierId;

        public Delivery()
        {
            CreatedDateTime = DateTime.UtcNow;
            _DeliveryStatusId = DeliveryStatus.New.Id;
        }

        public Delivery(Guid clientId, decimal price, short weight, string note) : this()
        {
            _clientId = clientId;
            Price = price;
            Weight = weight;
            Note = note;

            AddDomainEvent(new DeliveryCreatedDomainEvent(this, clientId));
        }

        public void SetPickUpLocation(DeliveryLocation pickUpLocation)
        {
            PickUpLocation = pickUpLocation;
        }

        public void SetDropOffLocation(DeliveryLocation dropOffLocation)
        {
            DropOffLocation = dropOffLocation;
        }

        public void SetDeliveryAvailableStatus()
        {
            var DeliveryStatusBeforeChange = _DeliveryStatusId;
            _DeliveryStatusId = DeliveryStatus.Available.Id;
            AddDomainEvent(new DeliveryStatusChangedToAvailableDomainEvent(Id, DeliveryStatusBeforeChange));
        }

        public void SetCourierId(Guid id)
        {
            _courierId = id;
            _DeliveryStatusId = DeliveryStatus.CourierAssigned.Id;
        }
    }
}
