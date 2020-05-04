using MediatR;
using System;

namespace Delivering.API.Application.Commands
{
    public class DeleteDeliveryCommand : IRequest<bool>
    {
        public Guid DeliveryId { get; set; }
    }
}
