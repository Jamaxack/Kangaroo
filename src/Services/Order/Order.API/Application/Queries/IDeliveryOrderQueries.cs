using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.API.Application.Queries
{
    public interface IDeliveryOrderQueries
    {
        Task<DeliveryOrderViewModel> GetDeliveryOrderByIdAsync(Guid id);
        Task<List<DeliveryOrderViewModel>> GetDeliveryOrdersAsync();
        Task<List<DeliveryOrderViewModel>> GetDeliveryOrdersByCourierIdAsync(Guid courierId);
        Task<List<DeliveryOrderViewModel>> GetDeliveryOrdersByClientIdAsync(Guid clientId);
    }
}
