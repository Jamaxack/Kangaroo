using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.API.Application.Commands
{
    public class SetAvailableDeliveryOrderStatusCommand : IRequest<bool>
    {
        public Guid DeliveryOrderId { get; set; }
    }
}
