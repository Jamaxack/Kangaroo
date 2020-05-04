using MediatR;
using Microsoft.Extensions.Logging;
using Delivering.Infrastructure.Idempotency;

namespace Delivering.API.Application.Commands
{
    // Use for Idempotency in Command process
    public class DeleteDeliveryIdentifiedCommandHandler : IdentifiedCommandHandler<DeleteDeliveryCommand, bool>
    {
        public DeleteDeliveryIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<DeleteDeliveryCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for creating Delivery.
        }
    }
}
