using System;
using Delivery.Domain.Common;
using Delivery.Domain.Events;

namespace Delivery.Domain.AggregatesModel.DeliveryAggregate
{
    public class Delivery : Entity, IAggregateRoot
    {
        private int _deliveryStatusId;

        public Delivery()
        {
            _deliveryStatusId = DeliveryStatus.New.Id;
        }

        public Delivery(Guid clientId, decimal price, short weight, string note, DateTime createdDateTime) : this()
        {
            GetClientId = clientId;
            Price = price;
            Weight = weight;
            Note = note;
            CreatedDateTime = createdDateTime;

            AddDomainEvent(new DeliveryCreatedDomainEvent(this, clientId));
        }

        public long Number { get; private set; }
        public DateTime CreatedDateTime { get; }
        public DateTime? FinishedDateTime { get; private set; }
        public decimal Price { get; }
        public short Weight { get; }
        public string Note { get; }
        public DeliveryStatus DeliveryStatus { get; private set; }
        public DeliveryLocation PickUpLocation { get; private set; }
        public DeliveryLocation DropOffLocation { get; private set; }
        public Guid GetClientId { get; }

        public Guid? GetCourierId { get; private set; }

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
            var deliveryStatusBeforeChange = _deliveryStatusId;
            _deliveryStatusId = DeliveryStatus.Available.Id;
            AddDomainEvent(new DeliveryStatusChangedToAvailableDomainEvent(Id, deliveryStatusBeforeChange));
        }

        public void SetCourierPickedUpDeliveryStatus()
        {
            var deliveryStatusBeforeChange = _deliveryStatusId;
            _deliveryStatusId = DeliveryStatus.CourierPickedUp.Id;
            AddDomainEvent(new DeliveryStatusChangedToCourierPickedUpDomainEvent(Id, deliveryStatusBeforeChange));
        }

        public void AssignCourier(Guid courierId)
        {
            GetCourierId = courierId;
            _deliveryStatusId = DeliveryStatus.CourierAssigned.Id;
        }
    }
}