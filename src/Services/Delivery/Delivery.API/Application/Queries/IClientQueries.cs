using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.API.Application.Queries
{
    public interface IClientQueries
    {
        Task<ClientViewModel> GetClientByIdAsync(Guid clientId);
        Task<List<ClientViewModel>> GetClientsAsync(int pageSize, int pageIndex);
    }
}
