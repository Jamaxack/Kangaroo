using Courier.API.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        readonly CourierContext _courierContext;

        public ClientRepository(IOptions<CourierSettings> settings)
            => _courierContext = new CourierContext(settings);

        public Task<Client> GetClientByIdAsync(Guid clientId)
        {
            var filter = Builders<Client>.Filter.Eq("_id", clientId);
            return _courierContext.Clients
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public Task InsertClientAsync(Client client)
            => _courierContext.Clients.InsertOneAsync(client);
    }
}
