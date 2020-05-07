namespace Courier.API.Infrastructure.Services
{
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

        public Task<Courier> GetCourierByIdAsync(Guid courierId)
        {
            return _courierRepository.GetCourierByIdAsync(courierId);
        }

        public Task<List<Courier>> GetCouriersAsync() => _courierRepository.GetCouriersAsync();

        public Task<CourierLocation> GetCurrentCourierLocationByCourierIdAsync(Guid courierId)
        {
            return _courierRepository.GetCurrentCourierLocationByCourierIdAsync(courierId);
        }

        public Task<List<Delivery>> GetDeliveriesByCourierIdAsync(Guid courierId)
        {
            return _courierRepository.GetDeliveriesByCourierIdAsync(courierId);
        }

        public Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId)
        {
            return _courierRepository.GetDeliveryByIdAsync(deliveryId);
        }

        public Task InsertCourierAsync(Courier courier)
        {
            return _courierRepository.InsertCourierAsync(courier);
        }


        public Task InsertCourierLocationAsync(CourierLocation courierLocation)
        {
            return _courierRepository.InsertCourierLocationAsync(courierLocation);
        }

        public Task InsertDeliveryAsync(Delivery delivery)
        {
            return _courierRepository.InsertDeliveryAsync(delivery);
        }

        public Task UpdateCourierAsync(Courier courier)
        {
            return _courierRepository.UpdateCourierAsync(courier);
        }
    }
}
