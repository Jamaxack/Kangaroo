using Delivering.API.Application.Models;
using Kangaroo.BuildingBlocks.EventBus.Events;
using System;

namespace Delivering.API.Application.IntegrationEvents.Events
{
    public class DeliveryCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid DeliveryId { get; set; }
        public long Number { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public decimal Price { get; set; }
        public short Weight { get; set; }
        public string Note { get; set; }
        public DeliveryLocation PickUpLocation { get; set; }
        public DeliveryLocation DropOffLocation { get; set; }
        public Guid ClientId { get; set; }
    }
}
