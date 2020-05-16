using MediatR;
using Microsoft.Extensions.Logging;
using Delivery.Infrastructure.Idempotency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.API.Application.Commands
{
    public class SetAvailableDeliveryStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetAvailableDeliveryStatusCommand, bool>
    {
        public SetAvailableDeliveryStatusIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<SetAvailableDeliveryStatusCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for creating Delivery.
        }
    }
}
