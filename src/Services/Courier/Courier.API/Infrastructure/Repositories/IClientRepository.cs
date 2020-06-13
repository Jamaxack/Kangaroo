using System;
using System.Threading.Tasks;
using Courier.API.Model;

namespace Courier.API.Infrastructure.Repositories
{
    public interface IClientRepository
    {
        Task InsertClientAsync(Client client);
        Task<Client> GetClientByIdAsync(Guid clientId);
    }
}