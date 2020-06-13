using System;
using System.Threading.Tasks;
using Delivery.Domain.AggregatesModel.ClientAggregate;
using Delivery.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DeliveryContext _context;

        public ClientRepository(DeliveryContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public Client Add(Client client)
        {
            if (client.IsTransient())
                return _context.Clients
                    .Add(client)
                    .Entity;
            return client;
        }

        public Client Update(Client client)
        {
            return _context.Clients
                .Update(client)
                .Entity;
        }

        public async Task<Client> FindByIdAsync(Guid id)
        {
            var client = await _context.Clients
                .SingleOrDefaultAsync(b => b.Id == id);

            return client;
        }
    }
}