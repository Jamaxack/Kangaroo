using MediatR;
using System;

namespace DeliveryOrder.API.Application.Commands
{
    public class DeleteDeliveryOrderCommand : IRequest<bool>
    {
        public Guid DeliveryOrderId { get; set; }
    }
}
