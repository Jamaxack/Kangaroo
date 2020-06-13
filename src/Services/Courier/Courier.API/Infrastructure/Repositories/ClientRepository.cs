using System;
using System.Threading.Tasks;
using Courier.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Courier.API.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly CourierContext _courierContext;

        public ClientRepository(IOptions<CourierSettings> settings)
        {
            _courierContext = new CourierContext(settings);
        }

        public Task<Client> GetClientByIdAsync(Guid clientId)
        {
            var filter = Builders<Client>.Filter.Eq("_id", clientId);
            return _courierContext.Clients
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public Task InsertClientAsync(Client client)
        {
            return _courierContext.Clients.InsertOneAsync(client);
        }
    }
}