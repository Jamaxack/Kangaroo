using MediatR;
using System;

namespace Delivery.API.Application.Commands
{
    public class DeleteDeliveryOrderCommand : IRequest<bool>
    {
        public Guid DeliveryOrderId { get; set; }
    }
}
