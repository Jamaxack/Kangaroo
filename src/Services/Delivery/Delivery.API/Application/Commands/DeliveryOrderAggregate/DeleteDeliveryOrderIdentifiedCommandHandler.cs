using MediatR;
using Microsoft.Extensions.Logging;
using Delivery.Infrastructure.Idempotency;

namespace Delivery.API.Application.Commands
{
    // Use for Idempotency in Command process
    public class DeleteDeliveryOrderIdentifiedCommandHandler : IdentifiedCommandHandler<DeleteDeliveryOrderCommand, bool>
    {
        public DeleteDeliveryOrderIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<DeleteDeliveryOrderCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for creating DeliveryOrder.
        }
    }
}
