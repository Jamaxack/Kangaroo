namespace Courier.API.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Courier.API.Model;

    public interface ICourierService
    {
        Task InsertCourierAsync(Courier courier);
        Task UpdateCourierAsync(Courier courier);
        Task<Courier> GetCourierByIdAsync(Guid courierId);
        Task<List<Courier>> GetCouriersAsync();

        Task InsertCourierLocationAsync(CourierLocation courierLocation);
        Task<CourierLocation> GetCurrentCourierLocationByCourierIdAsync(Guid courierId);

        Task InsertDeliveryAsync(Delivery delivery);
        Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId);
        Task<List<Delivery>> GetDeliveriesByCourierIdAsync(Guid courierId);
        Task DeleteDeliveryByIdAsync(Guid deliveryId);
    }
}
