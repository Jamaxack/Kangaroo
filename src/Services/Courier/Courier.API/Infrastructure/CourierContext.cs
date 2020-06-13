using Courier.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Courier.API.Infrastructure
{
    public class CourierContext
    {
        private readonly IMongoDatabase _database;

        public CourierContext(IOptions<CourierSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Model.Courier> Couriers => _database.GetCollection<Model.Courier>("Couriers");

        public IMongoCollection<Client> Clients => _database.GetCollection<Client>("Clients");

        public IMongoCollection<CourierLocation> CourierLocations =>
            _database.GetCollection<CourierLocation>("CourierLocations");

        public IMongoCollection<Delivery> Deliveries => _database.GetCollection<Delivery>("Deliveries");
    }
}