using MediatR;
using Microsoft.Extensions.Logging;
using Order.Infrastructure.Idempotency;

namespace Order.API.Application.Commands
{
    // Use for Idempotency in Command process
    public class CreateDeliveryOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateDeliveryOrderCommand, bool>
    {
        public CreateDeliveryOrderIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateDeliveryOrderCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for creating order.
        }
    }
}
