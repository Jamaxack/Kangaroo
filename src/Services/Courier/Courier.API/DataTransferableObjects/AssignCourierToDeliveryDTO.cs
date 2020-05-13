using System;

namespace Courier.API.DataTransferableObjects
{
    public class AssignCourierToDeliveryDTO
    {
        public Guid CourierId { get; set; }
        public Guid DelivertId { get; set; }
    }
}
