using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Exceptions;
using Courier.API.Infrastructure.Repositories;

namespace Courier.API.Infrastructure.Services
{
    public class CourierService : ICourierService
    {
        private readonly ICourierRepository _courierRepository;
        private readonly IMapper _mapper;

        public CourierService(ICourierRepository courierRepository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _courierRepository = courierRepository ?? throw new ArgumentNullException(nameof(courierRepository));
        }

        public async Task<CourierDto> GetCourierByIdAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
                throw new CourierDomainException("Courier Id is not specified");

            var courier = await _courierRepository.GetCourierByIdAsync(courierId);
            if (courier == null)
                throw new KeyNotFoundException($"Courier not found with specified Id: {courierId}");

            return _mapper.Map<CourierDto>(courier);
        }

        public async Task<List<CourierDto>> GetCouriersAsync()
        {
            var couriers = await _courierRepository.GetCouriersAsync();
            return _mapper.Map<List<CourierDto>>(couriers);
        }

        public async Task<List<DeliveryDto>> GetDeliveriesByCourierIdAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
                throw new CourierDomainException("Courier Id is not specified");

            var deliveries = await _courierRepository.GetDeliveriesByCourierIdAsync(courierId);
            return _mapper.Map<List<DeliveryDto>>(deliveries);
        }

        public Task InsertCourierAsync(CourierDtoSave courierDtoSave)
        {
            if (courierDtoSave == null)
                throw new CourierDomainException("Courier is null");

            var courier = _mapper.Map<Model.Courier>(courierDtoSave);
            return _courierRepository.InsertCourierAsync(courier);
        }

        public Task UpdateCourierAsync(CourierDtoSave courierDtoSave)
        {
            if (courierDtoSave == null)
                throw new CourierDomainException("Courier is null");

            var courier = _mapper.Map<Model.Courier>(courierDtoSave);
            return _courierRepository.UpdateCourierAsync(courier);
        }

        public async Task<CourierLocationDto> GetCurrentCourierLocationByCourierIdAsync(Guid courierId)
        {
            if (courierId == Guid.Empty)
                throw new CourierDomainException("Courier Id is not specified");

            var courierLocation = await _courierRepository.GetCurrentCourierLocationByCourierIdAsync(courierId);
            return _mapper.Map<CourierLocationDto>(courierLocation);
        }
    }
}