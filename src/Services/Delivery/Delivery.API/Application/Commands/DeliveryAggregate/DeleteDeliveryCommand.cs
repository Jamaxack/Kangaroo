using MediatR;
using System;

namespace Delivery.API.Application.Commands
{
    public class DeleteDeliveryCommand : IRequest<bool>
    {
        public Guid DeliveryId { get; set; }
    }
}
