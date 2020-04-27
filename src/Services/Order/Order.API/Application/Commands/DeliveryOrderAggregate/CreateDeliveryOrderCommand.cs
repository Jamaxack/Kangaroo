using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Order.API.Application.Commands
{
    public class CreateDeliveryOrderCommand : IRequest<bool>
    {
        public Guid ClientId { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public short Weight { get; set; }
        public string Note { get; set; }
        public bool ShouldNotifySenderOnOrderStatusChange { get; set; }
        public bool ShouldNotifyRecipientOnOrderStatusChange { get; set; }
        public List<DeliveryLocationDTO> DeliveryLocations { get; set; }

        public CreateDeliveryOrderCommand()
        {

        }
    }
}
