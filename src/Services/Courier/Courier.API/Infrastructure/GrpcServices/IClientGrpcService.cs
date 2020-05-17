using Courier.API.Model;
using System.Threading.Tasks;

namespace Courier.API.Infrastructure.GrpcServices
{
    public interface IClientGrpcService
    {
        Task<Client> GetClientByIdAsync(string clientId);
    }
}
