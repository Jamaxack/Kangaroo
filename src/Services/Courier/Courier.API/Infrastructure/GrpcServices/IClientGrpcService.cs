using System.Threading.Tasks;
using Courier.API.Model;

namespace Courier.API.Infrastructure.GrpcServices
{
    public interface IClientGrpcService
    {
        Task<Client> GetClientByIdAsync(string clientId);
    }
}