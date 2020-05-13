using Courier.API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Repositories
{
    public interface IDeliveryRepository
    {
        Task<List<Delivery>> GetAvailableDeliveriesAsync();
        Task InsertDeliveryAsync(Delivery delivery);
        Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId);
        Task DeleteDeliveryByIdAsync(Guid deliveryId);
        Task SetDeliveryStatusAsync(Guid deliveryId, DeliveryStatus deliveryStatus);
        Task AssignCourierToDeliveryAsync(Guid deliveryId, Guid courierId);
    }
}
