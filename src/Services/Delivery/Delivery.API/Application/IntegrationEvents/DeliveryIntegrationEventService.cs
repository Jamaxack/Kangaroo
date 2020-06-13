using System;
using System.Data.Common;
using System.Threading.Tasks;
using Delivery.Infrastructure;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Kangaroo.BuildingBlocks.EventBus.Events;
using Kangaroo.BuildingBlocks.IntegrationEventLogEF;
using Kangaroo.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Delivery.API.Application.IntegrationEvents
{
    public class DeliveryIntegrationEventService : IDeliveryIntegrationEventService
    {
        private readonly DeliveryContext _deliveryContext;
        private readonly IEventBus _eventBus;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<DeliveryIntegrationEventService> _logger;

        public DeliveryIntegrationEventService(IEventBus eventBus,
            DeliveryContext deliveryContext,
            IntegrationEventLogContext eventLogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            ILogger<DeliveryIntegrationEventService> logger)
        {
            _deliveryContext = deliveryContext ?? throw new ArgumentNullException(nameof(deliveryContext));
            var integrationEventLogServiceFactory1 = integrationEventLogServiceFactory ??
                                                     throw new ArgumentNullException(
                                                         nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = integrationEventLogServiceFactory1(_deliveryContext.Database.GetDbConnection());
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation(
                    "----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})",
                    logEvt.EventId, Program.AppName, logEvt.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}",
                        logEvt.EventId, Program.AppName);

                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent integrationEvent)
        {
            _logger.LogInformation(
                "----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})",
                integrationEvent.Id, integrationEvent);

            await _eventLogService.SaveEventAsync(integrationEvent, _deliveryContext.GetCurrentTransaction());
        }
    }
}