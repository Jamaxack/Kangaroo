using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Courier.API.Model;

namespace Courier.API.Infrastructure.Repositories
{
    public interface ICourierRepository
    {
        Task InsertCourierAsync(Model.Courier courier);
        Task UpdateCourierAsync(Model.Courier courier);
        Task<Model.Courier> GetCourierByIdAsync(Guid courierId);
        Task<List<Delivery>> GetDeliveriesByCourierIdAsync(Guid courierId);
        Task<List<Model.Courier>> GetCouriersAsync();
        Task<CourierLocation> GetCurrentCourierLocationByCourierIdAsync(Guid courierId);
    }
}