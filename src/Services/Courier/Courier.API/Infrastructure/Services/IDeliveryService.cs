using Courier.API.DataTransferableObjects;
using Courier.API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Services
{
    public interface IDeliveryService
    {
        Task<List<Delivery>> GetAvailableDeliveriesAsync();
        Task AssignCourierToDeliveryAsync(AssignCourierToDeliveryDTO assignCourierToDelivery);
        Task InsertDeliveryAsync(Delivery delivery);
        Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId);
        Task DeleteDeliveryByIdAsync(Guid deliveryId);
        Task DeliveryStatusChangedToCourierAssignedAsync(Guid deliveryId, Guid courierId);
        Task DeliveryStatusChangedToCourierPickedUpAsync(Guid deliveryId);
    }
}
