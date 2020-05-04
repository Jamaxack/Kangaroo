using Microsoft.EntityFrameworkCore;
using Delivering.Domain.AggregatesModel.ClientAggregate;
using Delivering.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Delivering.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        readonly DeliveringContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public ClientRepository(DeliveringContext context)
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
