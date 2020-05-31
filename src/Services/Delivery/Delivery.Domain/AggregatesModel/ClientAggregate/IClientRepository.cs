using Delivery.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Delivery.Domain.AggregatesModel.ClientAggregate
{
    public interface IClientRepository : IRepository<Client>
    {
        Client Add(Client client);
        Client Update(Client client);
        Task<Client> FindByIdAsync(Guid id);
    }
}
