using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delivery.API.Application.Queries.ViewModels;

namespace Delivery.API.Application.Queries
{
    public interface IClientQueries
    {
        Task<ClientViewModel> GetClientByIdAsync(Guid clientId);
        Task<List<ClientViewModel>> GetClientsAsync(int pageSize, int pageIndex);
    }
}
