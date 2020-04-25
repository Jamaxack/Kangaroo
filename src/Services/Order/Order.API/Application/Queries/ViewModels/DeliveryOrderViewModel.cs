using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using System;
using System.Collections.Generic;

namespace Order.API.Application.Queries
{
    public class DeliveryOrderViewModel
    {
        public Guid Id { get; set; } 
        public long Number { get; set; }
        public short Weight { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? FinishedDateTime { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string Note { get; set; } 
        public bool ShouldNotifySenderOnOrderStatusChange { get; set; }
        public bool ShouldNotifyRecipientOnOrderStatusChange { get; set; }
        public string DeliveryOrderStatus { get; set; }
        public Guid ClientId { get; set; }
        public Guid? CourierId { get; set; }
        public List<DeliveryLocationViewModel> DeliveryLocations { get; set; }

        public DeliveryOrderViewModel()
        {
            DeliveryLocations = new List<DeliveryLocationViewModel>();
        }
    }
}
