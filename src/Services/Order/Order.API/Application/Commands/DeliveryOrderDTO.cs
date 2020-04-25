using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Order.API.Application.Commands
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


        public static DeliveryOrderDTO FromOrder(DeliveryOrder order)
        {
            return new DeliveryOrderDTO()
            {
                Number = order.Number,
                Weight = order.Weight,
                CreatedDateTime = order.CreatedDateTime,
                FinishedDateTime = order.FinishedDateTime,
                PaymentAmount = order.PaymentAmount,
                InsuranceAmount = order.InsuranceAmount,
                Note = order.Note,
                DeliveryOrderNotificationSettings = order.DeliveryOrderNotificationSettings,
                DeliveryOrderStatus = order.DeliveryOrderStatus,
                ClientId = order.GetClientId,
                CourierId = order.GetCourierId,
                DeliveryLocations = order.DeliveryLocations.Select(location => DeliveryLocationDTO.FromLocation(location)),
            };
        }
    }
}
