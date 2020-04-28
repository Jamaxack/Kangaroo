using MediatR;
using Microsoft.Extensions.Logging;
using Order.Infrastructure.Idempotency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Application.Commands
{
    public class SetAvailableDeliveryOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetAvailableDeliveryOrderStatusCommand, bool>
    {
        public SetAvailableDeliveryOrderStatusIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<SetAvailableDeliveryOrderStatusCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for creating order.
        }
    }
}
