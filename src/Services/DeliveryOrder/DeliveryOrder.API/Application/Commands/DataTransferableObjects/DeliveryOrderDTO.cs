using DeliveryOrder.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryOrder.API.Application.Commands
{
    public class DeliveryOrderDTO
    {
        public long Number { get; set; }
        public short Weight { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? FinishedDateTime { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string Note { get; set; }
        // DeliveryOrderNotificationSettings is a Value Object pattern example persisted as EF Core 2.0 owned entity
        public DeliveryOrderNotificationSettings DeliveryOrderNotificationSettings { get; set; }
        public DeliveryOrderStatus DeliveryOrderStatus { get; set; }
        public Guid ClientId { get; set; }
        public Guid? CourierId { get; set; }
        public IEnumerable<DeliveryLocationDTO> DeliveryLocations { get; set; }


        public static DeliveryOrderDTO FromDeliveryOrder(Domain.AggregatesModel.DeliveryOrderAggregate.DeliveryOrder deliveryOrder)
        {
            return new DeliveryOrderDTO()
            {
                Number = deliveryOrder.Number,
                Weight = deliveryOrder.Weight,
                CreatedDateTime = deliveryOrder.CreatedDateTime,
                FinishedDateTime = deliveryOrder.FinishedDateTime,
                PaymentAmount = deliveryOrder.PaymentAmount,
                InsuranceAmount = deliveryOrder.InsuranceAmount,
                Note = deliveryOrder.Note,
                DeliveryOrderNotificationSettings = deliveryOrder.DeliveryOrderNotificationSettings,
                DeliveryOrderStatus = deliveryOrder.DeliveryOrderStatus,
                ClientId = deliveryOrder.GetClientId,
                CourierId = deliveryOrder.GetCourierId,
                DeliveryLocations = deliveryOrder.DeliveryLocations.Select(location => DeliveryLocationDTO.FromLocation(location)),
            };
        }
    }
}
