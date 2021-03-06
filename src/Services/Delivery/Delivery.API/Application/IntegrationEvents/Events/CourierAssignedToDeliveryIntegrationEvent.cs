﻿using System;
using Kangaroo.BuildingBlocks.EventBus.Events;

namespace Delivery.API.Application.IntegrationEvents.Events
{
    public class CourierAssignedToDeliveryIntegrationEvent : IntegrationEvent
    {
        public CourierAssignedToDeliveryIntegrationEvent(Guid deliveryId, Guid courierId)
        {
            DeliveryId = deliveryId;
            CourierId = courierId;
        }

        public Guid DeliveryId { get; set; }
        public Guid CourierId { get; set; }
    }
}