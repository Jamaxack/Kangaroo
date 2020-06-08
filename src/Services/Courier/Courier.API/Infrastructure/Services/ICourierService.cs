namespace Courier.API.Infrastructure.Services
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICourierService
    {
        Task InsertCourierAsync(Courier courier);
        Task UpdateCourierAsync(Courier courier);
        Task<Courier> GetCourierByIdAsync(Guid courierId);
        Task<List<Courier>> GetCouriersAsync();
        Task<List<Delivery>> GetDeliveriesByCourierIdAsync(Guid courierId);
        Task<CourierLocation> GetCurrentCourierLocationByCourierIdAsync(Guid courierId);
    }
}
