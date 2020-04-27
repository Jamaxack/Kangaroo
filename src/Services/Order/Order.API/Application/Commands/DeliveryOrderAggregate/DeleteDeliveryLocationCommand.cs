using MediatR;
using System;

namespace Order.API.Application.Commands
{
    public class DeleteDeliveryLocationCommand : IRequest<bool>
    {
        public Guid DeliveryOrderId { get; set; }
        public Guid DeliveryLocationId { get; set; }
    }
}
