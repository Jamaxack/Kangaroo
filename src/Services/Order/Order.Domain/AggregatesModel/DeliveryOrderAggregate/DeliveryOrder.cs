using Order.Domain.AggregatesModel.CourierAggregate;
using Order.Domain.Common;
using Order.Domain.Events;
using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.AggregatesModel.DeliveryOrderAggregate
{
    public class DeliveryOrder : Entity, IAggregateRoot
    {
        short _deliveryOrderStatusId;
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
        public DeliveryOrderStatus DeliveryOrderStatus => DeliveryOrderStatus.From(_deliveryOrderStatusId);
        public Guid GetClientId => _clientId;
        public Guid? GetCourierId => _courierId;
        public IReadOnlyCollection<DeliveryLocation> DeliveryLocations => _deliveryLocations;

        public DeliveryOrder()
        {
            _deliveryLocations = new List<DeliveryLocation>();
            CreatedDateTime = DateTime.UtcNow;
            _deliveryOrderStatusId = DeliveryOrderStatus.New.Id;
        }

        public DeliveryOrder(Guid identityGuid, Guid clientId, long number, DateTime createdDateTime, DateTime? finishedDateTime, decimal paymentAmount, decimal insuranceAmount, short weight, string note, DeliveryOrderNotificationSettings deliveryOrderNotificationSettings) : this()
        {
            _clientId = clientId;
            Number = number;
            CreatedDateTime = createdDateTime;
            FinishedDateTime = finishedDateTime;
            DeliveryOrderNotificationSettings = deliveryOrderNotificationSettings;
            PaymentAmount = paymentAmount;
            InsuranceAmount = insuranceAmount;
            Weight = weight;
            Note = note;

            AddDomainEvent(new NewDeliveryOrderCreatedDomainEvent(this, clientId));
        }

        public void AddDelivaryLocation(string address, string buildingNumber, string enterenceNumber, string floorNumber, string apartmentNumber,
            double latitude, double longitude, string note, decimal buyoutAmount, decimal takingAmount, bool isPaymentInThisDeliveryLocation, short deliveryLocationActionId,
            DateTime? arrivalStartDateTime, DateTime? arrivalFinishDateTime, DateTime? courierArrivedDateTime, ContactPerson contactPerson)
        {
            var deliveryLocation = new DeliveryLocation(address, buildingNumber, enterenceNumber, floorNumber, apartmentNumber, latitude,
                longitude, note, buyoutAmount, takingAmount, isPaymentInThisDeliveryLocation, deliveryLocationActionId, arrivalStartDateTime,
                arrivalFinishDateTime, courierArrivedDateTime, contactPerson);
            _deliveryLocations.Add(deliveryLocation);
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
