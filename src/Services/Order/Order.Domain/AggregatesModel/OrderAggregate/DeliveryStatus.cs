using Order.Domain.Common;
using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class DeliveryStatus : Enumeration
    {
        //Courier assigned, but still not departed
        public static DeliveryStatus CourierAssigned = new DeliveryStatus(1, nameof(CourierAssigned).ToLowerInvariant());
        //Courier departed to the pick-up point
        public static DeliveryStatus CourierDeparted = new DeliveryStatus(2, nameof(CourierDeparted).ToLowerInvariant());
        //Courier took parcel at the pick-up point
        public static DeliveryStatus CourierPickedUp = new DeliveryStatus(3, nameof(CourierPickedUp).ToLowerInvariant());
        //Courier has arrived and is waiting for a customer
        public static DeliveryStatus CourierArrived = new DeliveryStatus(4, nameof(CourierArrived).ToLowerInvariant());

        public DeliveryStatus(short id, string name)
            : base(id, name)
        { }

        public static IEnumerable<DeliveryStatus> List() => new[] { CourierAssigned, CourierDeparted, CourierPickedUp, CourierArrived };

        public static DeliveryStatus FromName(string name)
        {
            var state = List().SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
                throw new OrderingDomainException($"Possible values for DeliveryStatus: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }

        public static DeliveryStatus From(short id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
                throw new OrderingDomainException($"Possible values for DeliveryStatus: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }
    }
}
