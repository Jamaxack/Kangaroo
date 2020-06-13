using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Courier.API.Model;

namespace Courier.API.Infrastructure.Repositories
{
    public interface IDeliveryRepository
    {
        Task<List<Delivery>> GetAvailableDeliveriesAsync();
        Task<List<Delivery>> GetDeliveriesByCourierIdAsync(Guid courierId); 
        Task InsertDeliveryAsync(Delivery delivery);
        Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId);
        Task DeleteDeliveryByIdAsync(Guid deliveryId);
        Task SetDeliveryStatusAsync(Guid deliveryId, DeliveryStatus deliveryStatus);
        Task AssignCourierToDeliveryAsync(Guid deliveryId, Guid courierId);
    }
}