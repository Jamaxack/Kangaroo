using MediatR;
using System;

namespace DeliveryOrder.API.Application.Commands
{
    public class DeleteDeliveryLocationCommand : IRequest<bool>
    {
        public Guid DeliveryOrderId { get; set; }
        public Guid DeliveryLocationId { get; set; }
    }
}
