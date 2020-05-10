using AutoMapper;
using Courier.API.DataTransferableObjects;
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
        readonly IMapper _mapper;
        readonly ILogger<CourierLocationService> _logger;

        public CourierLocationService(ICourierLocationRepository courierLocationRepository, IEventBus eventBus, IMapper mapper, ILogger<CourierLocationService> logger)
        {
            _courierLocationRepository = courierLocationRepository;
            _eventBus = eventBus;
            _mapper = mapper;
            _logger = logger;
        }

        public Task InsertCourierLocationAsync(CourierLocationDTO courierLocationDTO)
        {
            var courierLocation = _mapper.Map<CourierLocation>(courierLocationDTO);
            courierLocation.DateTime = DateTime.UtcNow;
            return _courierLocationRepository.InsertCourierLocationAsync(courierLocation);
        }
    }
}
