using Courier.API.Infrastructure.Services;
using Courier.API.Model;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GrpcCourier
{
    public class CourierGrpcService : Courier.CourierBase
    {
        readonly IDeliveryService _deliveryService;
        readonly ILogger<CourierGrpcService> _logger;

        public CourierGrpcService(IDeliveryService deliveryService, ILogger<CourierGrpcService> logger)
        {
            _deliveryService = deliveryService;
            _logger = logger;
        }

        public override async Task<DeliveryResponse> GetDeliveryById(DeliveryRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for delivery id {Id}", context.Method, request.DeliveryId);

            if (Guid.TryParse(request.DeliveryId, out Guid deliveryId))
            {
                var delivery = await _deliveryService.GetDeliveryByIdAsync(deliveryId);
                if (delivery == null)
                {
                    context.Status = new Status(StatusCode.NotFound, $"Delivery with id {request.DeliveryId} do not exists");
                }
                else
                {
                    context.Status = new Status(StatusCode.OK, $"Delivery with id {request.DeliveryId} exists");
                    return MapToDeliveryResponse(delivery);
                }
            }
            else
                context.Status = new Status(StatusCode.InvalidArgument, $"{nameof(request.DeliveryId)} must be Guid. Not able to parse '{request.DeliveryId}' to Guid");

            return new DeliveryResponse();
        }

        DeliveryResponse MapToDeliveryResponse(Delivery delivery)
        {
            return new DeliveryResponse
            {
                DeliveryId = delivery.DeliveryId.ToString(),
                Number = delivery.Number,
                Note = delivery.Note,
                Status = delivery.DeliveryStatus.ToString(),
            };
        }
    }
}
