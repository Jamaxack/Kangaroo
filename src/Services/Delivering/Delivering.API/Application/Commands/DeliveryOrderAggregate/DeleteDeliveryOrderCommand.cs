using MediatR;
using System;

namespace Delivering.API.Application.Commands
{
    public class DeleteDeliveryOrderCommand : IRequest<bool>
    {
        public Guid DeliveryOrderId { get; set; }
    }
}
