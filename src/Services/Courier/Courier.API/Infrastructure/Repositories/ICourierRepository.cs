namespace Courier.API.Infrastructure.Repositories
{
    using Courier.API.Model;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICourierRepository
    {
        Task InsertCourierAsync(Courier courier);
        Task UpdateCourierAsync(Courier courier);
        Task<Courier> GetCourierByIdAsync(Guid courierId);
        Task<List<Delivery>> GetDeliveriesByCourierIdAsync(Guid courierId);
        Task<List<Courier>> GetCouriersAsync();
        Task<CourierLocation> GetCurrentCourierLocationByCourierIdAsync(Guid courierId);
    }
}
