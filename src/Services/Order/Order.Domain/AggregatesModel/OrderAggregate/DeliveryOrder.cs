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
        short _orderStatusId; 
        Guid _clientId;
        Guid? _courierId;
        readonly List<DeliveryLocation> _deliveryLocations;
        readonly List<OrderStatus> _orderStatusHistory; 

        public long Number { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime? FinishedDateTime { get; private set; }
        public decimal PaymentAmount { get; private set; }
        public decimal InsuranceAmount { get; private set; }
        public short Weight { get; private set; }
        public string Note { get; private set; }
        // DeliveryOrderNotificationSettings is a Value Object pattern example persisted as EF Core 2.0 owned entity
        public DeliveryOrderNotificationSettings DeliveryOrderNotificationSettings { get; private set; } 
        public OrderStatus OrderStatus => OrderStatus.From(_orderStatusId);
        public Guid GetClientId => _clientId;
        public Guid? GetCourierId => _courierId;
        public IReadOnlyCollection<DeliveryLocation> DeliveryLocations => _deliveryLocations;
        public IReadOnlyCollection<OrderStatus> OrderStatusHistory => _orderStatusHistory; 

        public DeliveryOrder()
        {
            _deliveryLocations = new List<DeliveryLocation>();
            _orderStatusHistory = new List<OrderStatus>(); 
            CreatedDateTime = DateTime.UtcNow;
            _orderStatusId = OrderStatus.New.Id;
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

            //TODO: Publish DeliveryOrder added DomainEvent with identityGuid
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
            _orderStatusId = OrderStatus.Available.Id;
        }

        public void SetCourierId(Guid id)
        {
            _courierId = id; 
            _orderStatusId = OrderStatus.CourierAssigned.Id;
            //TODO: set status history  
        }

        private void StatusChangeException(OrderStatus orderStatusToChange)
        {
            throw new OrderingDomainException($"Is not possible to change the order status from {OrderStatus.Name} to {orderStatusToChange.Name}.");
        }
    }
}
