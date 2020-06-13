using System;
using MediatR;

namespace Delivery.API.Application.Commands.DeliveryAggregate
{
    public class SetAvailableDeliveryStatusCommand : IRequest<bool>
    {
        public Guid DeliveryId { get; set; }
    }
}
