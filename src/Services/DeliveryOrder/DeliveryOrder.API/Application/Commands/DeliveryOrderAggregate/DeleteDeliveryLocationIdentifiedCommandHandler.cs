using MediatR;
using Microsoft.Extensions.Logging;
using DeliveryOrder.Infrastructure.Idempotency;

namespace DeliveryOrder.API.Application.Commands
{
    // Use for Idempotency in Command process
    public class DeleteDeliveryLocationIdentifiedCommandHandler : IdentifiedCommandHandler<DeleteDeliveryLocationCommand, bool>
    {
        public DeleteDeliveryLocationIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<DeleteDeliveryLocationCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for creating DeliveryOrder.
        }
    }
}
