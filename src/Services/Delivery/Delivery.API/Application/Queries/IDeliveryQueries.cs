using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.API.Application.Queries.ViewModels;

namespace Delivery.API.Application.Queries
{
    public interface IDeliveryQueries
    {
        Task<DeliveryViewModel> GetDeliveryByIdAsync(Guid id);
        Task<List<DeliveryViewModel>> GetDeliverysAsync(int pageSize, int pageIndex);
        Task<List<DeliveryViewModel>> GetDeliverysByCourierIdAsync(Guid courierId);
        Task<List<DeliveryViewModel>> GetDeliverysByClientIdAsync(Guid clientId);
    }
}
