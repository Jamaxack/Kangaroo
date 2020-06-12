using System;

namespace Courier.API.DataTransferableObjects
{
    public class AssignCourierToDeliveryDtoSave
    {
        public Guid CourierId { get; set; }
        public Guid DeliveryId { get; set; }
    }
}
