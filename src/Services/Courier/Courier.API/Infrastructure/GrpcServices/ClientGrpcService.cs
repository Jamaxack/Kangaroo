using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Courier.API.Configurations;
using Courier.API.Model;
using Grpc.Net.Client;
using GrpcClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Courier.API.Infrastructure.GrpcServices
{
    public class ClientGrpcService : IClientGrpcService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ClientGrpcService> _logger;
        private readonly UrlsConfiguration _urls;

        public ClientGrpcService(HttpClient httpClient, IOptions<UrlsConfiguration> config,
            ILogger<ClientGrpcService> logger)
        {
            _httpClient = httpClient;
            _urls = config.Value;
            _logger = logger;
        }

        public async Task<Client> GetClientByIdAsync(string clientId)
        {
            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            using var channel = GrpcChannel.ForAddress(_urls.GrpcDelivery);
            var client = new ClientGrpc.ClientGrpcClient(channel);

            _logger.LogDebug("Grpc client created, request = {@id}", clientId);
            var response = await client.GetClientByIdAsync(new ClientRequest {ClientId = clientId});
            _logger.LogDebug("Grpc response {@response}", response);

            var delivery = MapToDelivery(response);
            if (delivery == null)
                throw new KeyNotFoundException($"Client not found with specified Id: {clientId}");

            return delivery;
        }

        private Client MapToDelivery(ClientResponse response)
        {
            return new Client
            {
                Id = Guid.Parse(response.ClientId),
                Phone = response.Phone,
                FullName = response.FullName
            };
        }
    }
}