namespace Courier.API.Infrastructure.Services
{
    using Exceptions;
    using Repositories;
    using Model;
    using Kangaroo.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CourierService : ICourierService
    {
        readonly ICourierRepository _courierRepository;
        readonly IEventBus _eventBus;
        readonly ILogger<CourierService> _logger;

        public CourierService(ICourierRepository courierRepository, IEventBus eventBus, ILogger<CourierService> logger)
        {
            _courierRepository = courierRepository ?? throw new ArgumentNullException(nameof(courierRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Courier> GetCourierByIdAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
                throw new CourierDomainException("Courier Id is not specified");

            var courier = await _courierRepository.GetCourierByIdAsync(courierId);
            if (courier == null)
                throw new KeyNotFoundException($"Courier not found with specified Id: {courierId}");

            return courier;
        }

        public Task<List<Courier>> GetCouriersAsync() => _courierRepository.GetCouriersAsync();

        public Task<List<Delivery>> GetDeliveriesByCourierIdAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
                throw new CourierDomainException("Courier Id is not specified");

            return _courierRepository.GetDeliveriesByCourierIdAsync(courierId);
        }

        public Task InsertCourierAsync(Courier courier)
        {
            if (courier == null)
                throw new CourierDomainException("Courier is null");

            return _courierRepository.InsertCourierAsync(courier);
        }

        public Task UpdateCourierAsync(Courier courier)
        {
            if (courier == null)
                throw new CourierDomainException("Courier is null");

            return _courierRepository.UpdateCourierAsync(courier);
        }

        public Task<CourierLocation> GetCurrentCourierLocationByCourierIdAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
                throw new CourierDomainException("Courier Id is not specified");

            return _courierRepository.GetCurrentCourierLocationByCourierIdAsync(courierId);
        }
    }
}
