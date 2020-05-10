using Courier.API.Infrastructure.Repositories;
using Courier.API.Model;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Services
{
    public class CourierLocationService : ICourierLocationService
    {
        readonly ICourierLocationRepository _courierLocationRepository;
        readonly IEventBus _eventBus;
        readonly ILogger<CourierLocationService> _logger;

        public CourierLocationService(ICourierLocationRepository courierLocationRepository, IEventBus eventBus, ILogger<CourierLocationService> logger)
        {
            _courierLocationRepository = courierLocationRepository;
            _eventBus = eventBus;
            _logger = logger;
        }

        public Task InsertCourierLocationAsync(CourierLocation courierLocation)
        {
            courierLocation.DateTime = DateTime.UtcNow;
            return _courierLocationRepository.InsertCourierLocationAsync(courierLocation);
        }

    }
}
