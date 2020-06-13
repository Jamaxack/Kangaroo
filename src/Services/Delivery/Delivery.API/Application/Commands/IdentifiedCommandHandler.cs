using System;
using System.Threading;
using System.Threading.Tasks;
using Delivery.API.Application.Commands.DeliveryAggregate;
using Delivery.Infrastructure.Idempotency;
using Kangaroo.BuildingBlocks.EventBus.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Delivery.API.Application.Commands
{
    /// <summary>
    ///     Provides a base implementation for handling duplicate request and ensuring idempotent updates, in the cases where
    ///     a request id sent by client is used to detect duplicate requests.
    /// </summary>
    /// <typeparam name="TRequest">Type of the command handler that performs the operation if request is not duplicated</typeparam>
    /// <typeparam name="TResponse">Return value of the inner command handler</typeparam>
    public class
        IdentifiedCommandHandler<TRequest, TResponse> : IRequestHandler<IdentifiedCommand<TRequest, TResponse>,
            TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<IdentifiedCommandHandler<TRequest, TResponse>> _logger;
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;

        public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<TRequest, TResponse>> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            ;
            _requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
            ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ;
        }

        /// <summary>
        ///     This method handles the command. It just ensures that no other request exists with the same ID, and if this is the
        ///     case
        ///     just enqueues the original inner command.
        /// </summary>
        /// <param name="message">IdentifiedCommand which contains both original command & request ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Return value of inner command or default value if request same ID was found</returns>
        public async Task<TResponse> Handle(IdentifiedCommand<TRequest, TResponse> message,
            CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(message.Id);
            if (alreadyExists) return CreateResultForDuplicateRequest();

            await _requestManager.CreateRequestForCommandAsync<TRequest>(message.Id);
            try
            {
                var command = message.Command;
                var commandName = command.GetGenericTypeName();
                var idProperty = string.Empty;
                var commandId = Guid.Empty;

                switch (command)
                {
                    case CreateDeliveryCommand createDeliveryCommand:
                        idProperty = nameof(createDeliveryCommand.ClientId);
                        commandId = createDeliveryCommand.ClientId;
                        break;
                    case DeleteDeliveryCommand deleteDeliveryCommand:
                        idProperty = nameof(deleteDeliveryCommand.DeliveryId);
                        commandId = deleteDeliveryCommand.DeliveryId;
                        break;
                }

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    commandName,
                    idProperty,
                    commandId,
                    command);

                // Send the embeded business command to mediator so it runs its related CommandHandler 
                var result = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation(
                    "----- Command result: {@Result} - {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    result,
                    commandName,
                    idProperty,
                    commandId,
                    command);

                return result;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        ///     Creates the result value to return if a previous request was found
        /// </summary>
        /// <returns></returns>
        protected virtual TResponse CreateResultForDuplicateRequest()
        {
            return default;
        }
    }
}