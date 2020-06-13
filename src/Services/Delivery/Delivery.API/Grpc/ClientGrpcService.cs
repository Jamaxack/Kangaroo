using System;
using System.Threading.Tasks;
using Delivery.API.Application.Queries;
using Delivery.API.Application.Queries.ViewModels;
using Grpc.Core;
using GrpcClient;
using Microsoft.Extensions.Logging;

namespace Delivery.API.Grpc
{
    public class ClientGrpcService : ClientGrpc.ClientGrpcBase
    {
        private readonly IClientQueries _clientQueries;
        private readonly ILogger<ClientGrpcService> _logger;

        public ClientGrpcService(IClientQueries clientQueries, ILogger<ClientGrpcService> logger)
        {
            _clientQueries = clientQueries;
            _logger = logger;
        }

        public override async Task<ClientResponse> GetClientById(ClientRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for client id {Id}", context.Method,
                request.ClientId);

            if (Guid.TryParse(request.ClientId, out var clientId))
            {
                var client = await _clientQueries.GetClientByIdAsync(clientId);
                if (client == null)
                {
                    context.Status = new Status(StatusCode.NotFound, $"Client with id {clientId} do not exists");
                }
                else
                {
                    context.Status = new Status(StatusCode.OK, $"Client with id {clientId} exists");
                    return MapToClientResponse(client);
                }
            }
            else
            {
                context.Status = new Status(StatusCode.InvalidArgument,
                    $"{nameof(request.ClientId)} must be Guid. Not able to parse '{request.ClientId}' to Guid");
            }

            return new ClientResponse();
        }

        private ClientResponse MapToClientResponse(ClientViewModel client)
        {
            return new ClientResponse
            {
                ClientId = client.Id.ToString(),
                Phone = client.Phone,
                FullName = $"{client.FirstName} {client.LastName}"
            };
        }
    }
}