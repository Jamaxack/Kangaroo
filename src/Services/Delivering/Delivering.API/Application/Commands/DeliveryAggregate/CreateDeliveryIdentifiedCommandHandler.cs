using MediatR;
using Microsoft.Extensions.Logging;
using Delivering.Infrastructure.Idempotency;

namespace Delivering.API.Application.Commands
{
    // Use for Idempotency in Command process
    public class CreateDeliveryIdentifiedCommandHandler : IdentifiedCommandHandler<CreateDeliveryCommand, bool>
    {
        public CreateDeliveryIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateDeliveryCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for creating Delivery.
        }
    }
}
