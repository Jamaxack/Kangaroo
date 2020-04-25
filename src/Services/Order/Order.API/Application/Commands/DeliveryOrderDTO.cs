using Order.Domain.AggregatesModel.DeliveryOrderAggregate;
using System.Collections.Generic;
using System.Linq;

namespace Order.API.Application.Commands
{
    public class DeliveryOrderDTO
    {
        public IEnumerable<DeliveryLocationDTO> DeliveryLocations { get; set; }
        public long Number { get; set; }
        public short Weight { get; set; }

        public static DeliveryOrderDTO FromOrder(DeliveryOrder order)
        {
            return new DeliveryOrderDTO()
            {
                DeliveryLocations = order.DeliveryLocations.Select(x => new DeliveryLocationDTO
                {
                    Address = x.Address,
                    Note = x.Note
                }),
                Number = order.Number,
                Weight = order.Weight
            };
        }
    }
}
