using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Courier.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Courier.API.Infrastructure.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly CourierContext _courierContext;

        public DeliveryRepository(IOptions<CourierSettings> settings)
        {
            _courierContext = new CourierContext(settings);
        }

        public Task<List<Delivery>> GetAvailableDeliveriesAsync()
        {
            var filter = Builders<Delivery>.Filter.Eq("DeliveryStatus", DeliveryStatus.Available);
            return _courierContext.Deliveries.Find(filter).ToListAsync();
        }

        public Task AssignCourierToDeliveryAsync(Guid deliveryId, Guid courierId)
        {
            var filter = Builders<Delivery>.Filter.Eq("DeliveryId", deliveryId);
            var update = Builders<Delivery>.Update.Set(delivery => delivery.CourierId, courierId);
            var options = new FindOneAndUpdateOptions<Delivery>();
            return _courierContext.Deliveries.FindOneAndUpdateAsync(filter, update, options);
        }

        public Task InsertDeliveryAsync(Delivery delivery)
        {
            return _courierContext.Deliveries.InsertOneAsync(delivery);
        }

        public Task<Delivery> GetDeliveryByIdAsync(Guid deliveryId)
        {
            var filter = Builders<Delivery>.Filter.Eq("DeliveryId", deliveryId);
            return _courierContext.Deliveries
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public Task DeleteDeliveryByIdAsync(Guid deliveryId)
        {
            var deleteFilter = Builders<Delivery>.Filter.Eq("DeliveryId", deliveryId);
            return _courierContext.Deliveries.DeleteOneAsync(deleteFilter);
        }

        public Task SetDeliveryStatusAsync(Guid deliveryId, DeliveryStatus deliveryStatus)
        {
            var filter = Builders<Delivery>.Filter.Eq("DeliveryId", deliveryId);
            var update = Builders<Delivery>.Update.Set(delivery => delivery.DeliveryStatus, deliveryStatus);
            var options = new FindOneAndUpdateOptions<Delivery>();
            return _courierContext.Deliveries.FindOneAndUpdateAsync(filter, update, options);
        }
    }
}