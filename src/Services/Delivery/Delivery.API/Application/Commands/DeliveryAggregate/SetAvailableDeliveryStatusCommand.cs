using MediatR;
using System;

namespace Delivery.API.Application.Commands
{
    public class SetAvailableDeliveryStatusCommand : IRequest<bool>
    {
        public Guid DeliveryId { get; set; }
    }
}
