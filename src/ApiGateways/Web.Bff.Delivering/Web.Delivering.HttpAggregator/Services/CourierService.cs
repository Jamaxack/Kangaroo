using Grpc.Net.Client;
using GrpcCourier;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Delivering.HttpAggregator.Config;
using Web.Delivering.HttpAggregator.Models;

namespace Web.Delivering.HttpAggregator.Services
{
    public class CourierService : ICourierService
    {
        readonly UrlsConfig _urls;
        readonly ILogger<CourierService> _logger;

        public CourierService(HttpClient httpClient, IOptions<UrlsConfig> config, ILogger<CourierService> logger)
        {
            _urls = config.Value;
            _logger = logger;
        }

        public async Task<Delivery> GetDeliveryByIdAsync(string deliveryId)
        {
            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            using var channel = GrpcChannel.ForAddress(_urls.GrpcCourier);
            var client = new Courier.CourierClient(channel);

            _logger.LogDebug("Grpc client created, request = {@id}", deliveryId);
            var response = await client.GetDeliveryByIdAsync(new DeliveryRequest { DeliveryId = deliveryId });
            _logger.LogDebug("Grpc response {@response}", response);

            return MapToDelivery(response);
        }

        Delivery MapToDelivery(DeliveryResponse response)
            => new Delivery
            {
                DeliveryId = Guid.Parse(response.DeliveryId),
                Note = response.Note,
                Number = response.Number,
                Status = response.Status
            };
    }
}
