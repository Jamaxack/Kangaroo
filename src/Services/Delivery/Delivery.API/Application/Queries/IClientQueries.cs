using System;
using System.Threading.Tasks;

namespace Delivery.API.Application.Queries
{
    public interface IClientQueries
    {
        Task<ClientViewModel> GetClientByIdAsync(Guid clientId);
    }
}
