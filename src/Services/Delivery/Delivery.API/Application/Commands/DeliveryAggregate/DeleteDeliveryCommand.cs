using System;
using MediatR;

namespace Delivery.API.Application.Commands.DeliveryAggregate
{
    public class DeleteDeliveryCommand : IRequest<bool>
    {
        public Guid DeliveryId { get; set; }
    }
}