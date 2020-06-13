using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Courier.API.DataTransferableObjects;

namespace Courier.API.Infrastructure.Services
{
    public interface ICourierService
    {
        Task InsertCourierAsync(CourierDtoSave courier);
        Task UpdateCourierAsync(CourierDtoSave courierDtoSave);
        Task<CourierDto> GetCourierByIdAsync(Guid courierId);
        Task<List<CourierDto>> GetCouriersAsync();
        Task<List<DeliveryDto>> GetDeliveriesByCourierIdAsync(Guid courierId);
        Task<CourierLocationDto> GetCurrentCourierLocationByCourierIdAsync(Guid courierId);
    }
}