namespace Courier.API.Infrastructure.Services
{
    using Courier.API.Infrastructure.Exceptions;
    using Courier.API.Infrastructure.Repositories;
    using Courier.API.Model;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CourierService : ICourierService
    {
        readonly ICourierRepository _courierRepository;
        readonly ILogger<CourierService> _logger;

        public CourierService(ICourierRepository courierRepository, ILogger<CourierService> logger)
        {
            _courierRepository = courierRepository ?? throw new ArgumentNullException(nameof(courierRepository));
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

        public Task<CourierLocation> GetCurrentCourierLocationByCourierIdAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
                throw new CourierDomainException("Courier Id is not specified");
             
            return _courierRepository.GetCurrentCourierLocationByCourierIdAsync(courierId);
        }

        public Task<List<Delivery>> GetDeliveriesByCourierIdAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
                throw new CourierDomainException("Courier Id is not specified");

            return _courierRepository.GetDeliveriesByCourierIdAsync(courierId);
        }

        public Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId)
        {
            if (deliveryId == Guid.Empty)
                throw new CourierDomainException("Delivery Id is not specified");

            var delivery = _courierRepository.GetDeliveryByIdAsync(deliveryId);
            if (delivery == null)
                throw new KeyNotFoundException();
            return delivery;
        }

        public Task InsertCourierAsync(Courier courier)
        {
            if (courier == null)
                throw new CourierDomainException("Courier is null");

            return _courierRepository.InsertCourierAsync(courier);
        }

        public Task InsertCourierLocationAsync(CourierLocation courierLocation)
        {
            courierLocation.DateTime = DateTime.UtcNow;
            return _courierRepository.InsertCourierLocationAsync(courierLocation);
        }

        public Task InsertDeliveryAsync(Delivery delivery)
        {
            if (delivery == null)
                throw new CourierDomainException("Delivery is null");

            return _courierRepository.InsertDeliveryAsync(delivery);
        }

        public Task DeleteDeliveryByIdAsync(Guid deliveryId)
        {
            if (deliveryId == null)
                throw new CourierDomainException("Delivery Id is not specified");

            return _courierRepository.DeleteDeliveryByIdAsync(deliveryId);
        }

        public Task UpdateCourierAsync(Courier courier)
        {
            if (courier == null)
                throw new CourierDomainException("Courier is null");

            return _courierRepository.UpdateCourierAsync(courier);
        }
    }
}
