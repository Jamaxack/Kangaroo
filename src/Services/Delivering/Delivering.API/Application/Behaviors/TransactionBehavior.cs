using Delivering.API.Application.IntegrationEvents;
using Delivering.Infrastructure;
using Kangaroo.BuildingBlocks.EventBus.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Delivering.API.Application.Behaviors
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        readonly DeliveringContext _dbContext;
        readonly IDeliveringIntegrationEventService _deliveringIntegrationEventService;

        public TransactionBehaviour(DeliveringContext dbContext,
            IDeliveringIntegrationEventService deliveringIntegrationEventService,
            ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(DeliveringContext));
            _deliveringIntegrationEventService = deliveringIntegrationEventService ?? throw new ArgumentException(nameof(deliveringIntegrationEventService));
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;

                    using (var transaction = await _dbContext.BeginTransactionAsync())
                    {
                        _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                        response = await next();

                        _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                        await _dbContext.CommitTransactionAsync(transaction);

                        transactionId = transaction.TransactionId;
                    }

                    await _deliveringIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

                throw;
            }
        }
    }
}
