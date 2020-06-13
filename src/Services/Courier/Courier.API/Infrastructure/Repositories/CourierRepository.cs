using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Courier.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Courier.API.Infrastructure.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly CourierContext _courierContext;

        public CourierRepository(IOptions<CourierSettings> settings)
        {
            _courierContext = new CourierContext(settings);
        }

        public Task InsertCourierAsync(Model.Courier courier)
        {
            return _courierContext.Couriers.InsertOneAsync(courier);
        }

        public Task UpdateCourierAsync(Model.Courier courier)
        {
            return _courierContext.Couriers.ReplaceOneAsync(
                doc => doc.Id == courier.Id,
                courier,
                new ReplaceOptions {IsUpsert = true});
        }

        public Task<Model.Courier> GetCourierByIdAsync(Guid courierId)
        {
            var filter = Builders<Model.Courier>.Filter.Eq("_id", courierId);
            return _courierContext.Couriers
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public Task<List<Model.Courier>> GetCouriersAsync()
        {
            return _courierContext.Couriers.Find(new BsonDocument()).ToListAsync();
            ;
        }

        public Task<CourierLocation> GetCurrentCourierLocationByCourierIdAsync(Guid courierId)
        {
            //Get last inserted document, so it is current courier location
            var filter = Builders<CourierLocation>.Filter.Eq("CourierId", courierId);
            return _courierContext.CourierLocations
                .Find(filter)
                .SortByDescending(x => x.DateTime)
                .FirstOrDefaultAsync();
        }

        public Task<List<Delivery>> GetDeliveriesByCourierIdAsync(Guid courierId)
        {
            var filter = Builders<Delivery>.Filter.Eq("CourierId", courierId);
            return _courierContext.Deliveries
                .Find(filter)
                .ToListAsync();
        }
    }
}