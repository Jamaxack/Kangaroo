using Microsoft.EntityFrameworkCore;
using Delivery.Domain.AggregatesModel.ClientAggregate;
using Delivery.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Delivery.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        readonly DeliveryContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public ClientRepository(DeliveryContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public Client Add(Client client)
        {
            if (client.IsTransient())
            {
                return _context.Clients
                    .Add(client)
                    .Entity;
            }
            else
            {
                return client;
            }
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
