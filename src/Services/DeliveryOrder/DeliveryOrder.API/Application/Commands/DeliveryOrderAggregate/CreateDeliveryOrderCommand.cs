using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DeliveryOrder.API.Application.Commands
{
    public class CreateDeliveryOrderCommand : IRequest<bool>
    {
        public Guid ClientId { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public short Weight { get; set; }
        public string Note { get; set; }
        public bool ShouldNotifySenderOnDeliveryOrderStatusChange { get; set; }
        public bool ShouldNotifyRecipientOnDeliveryOrderStatusChange { get; set; }
        public List<DeliveryLocationDTO> DeliveryLocations { get; set; }

        public CreateDeliveryOrderCommand()
        {

        }
    }
}
