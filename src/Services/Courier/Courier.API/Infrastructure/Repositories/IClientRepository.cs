using Courier.API.Model;
using System;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.Repositories
{
    public interface IClientRepository
    {
        Task InsertClientAsync(Client client);
        Task<Client> GetClientByIdAsync(Guid clientId);
    }
}
