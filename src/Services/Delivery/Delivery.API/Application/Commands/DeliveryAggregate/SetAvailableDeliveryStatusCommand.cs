using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.API.Application.Commands
{
    public class SetAvailableDeliveryStatusCommand : IRequest<bool>
    {
        public Guid DeliveryId { get; set; }
    }
}
