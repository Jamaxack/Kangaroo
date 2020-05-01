using Microsoft.EntityFrameworkCore;
using DeliveryOrder.Domain.AggregatesModel.ClientAggregate;
using DeliveryOrder.Domain.Common;
using System;
using System.Threading.Tasks;

namespace DeliveryOrder.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        readonly DeliveryOrderContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public ClientRepository(DeliveryOrderContext context)
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
