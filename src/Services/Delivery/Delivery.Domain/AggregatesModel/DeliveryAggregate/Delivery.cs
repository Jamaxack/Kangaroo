﻿using Delivery.Domain.Common;
using Delivery.Domain.Events;
using System;

namespace Delivery.Domain.AggregatesModel.DeliveryAggregate
{
    public class Delivery : Entity, IAggregateRoot
    {
        public long Number { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime? FinishedDateTime { get; private set; }
        public decimal Price { get; private set; }
        public short Weight { get; private set; }
        public string Note { get; private set; }

        int _deliveryStatusId;
        public DeliveryStatus DeliveryStatus { get; private set; }
        public DeliveryLocation PickUpLocation { get; private set; }
        public DeliveryLocation DropOffLocation { get; private set; }

        Guid _clientId;
        public Guid GetClientId => _clientId;

        Guid? _courierId;
        public Guid? GetCourierId => _courierId;

        public Delivery()
        {
            _deliveryStatusId = DeliveryStatus.New.Id;
        }

        public Delivery(Guid clientId, decimal price, short weight, string note, DateTime createdDateTime) : this()
        {
            _clientId = clientId;
            Price = price;
            Weight = weight;
            Note = note;
            CreatedDateTime = createdDateTime;

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
            _courierId = courierId;
            _deliveryStatusId = DeliveryStatus.CourierAssigned.Id;
        }
    }
}
