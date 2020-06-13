using Delivery.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Delivery.API.Application.Commands.DeliveryAggregate
{
    public class
        SetAvailableDeliveryStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetAvailableDeliveryStatusCommand,
            bool>
    {
        public SetAvailableDeliveryStatusIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<SetAvailableDeliveryStatusCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for creating Delivery.
        }
    }
}