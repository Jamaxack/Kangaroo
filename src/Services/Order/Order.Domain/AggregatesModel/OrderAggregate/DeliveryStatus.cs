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
        //Planned delivery (No courier assigned)
        public static DeliveryStatus Planned = new DeliveryStatus(1, nameof(Planned).ToLowerInvariant());
        //Courier assigned, but still not departed
        public static DeliveryStatus CourierAssigned = new DeliveryStatus(2, nameof(CourierAssigned).ToLowerInvariant());
        //Courier departed to the pick-up point
        public static DeliveryStatus CourierDeparted = new DeliveryStatus(3, nameof(CourierDeparted).ToLowerInvariant());
        //Courier took parcel at the pick-up point
        public static DeliveryStatus CourierPickedUp = new DeliveryStatus(4, nameof(CourierPickedUp).ToLowerInvariant());
        //Courier has arrived and is waiting for a customer
        public static DeliveryStatus CourierArrived = new DeliveryStatus(5, nameof(CourierArrived).ToLowerInvariant());
        //Delivery failed (Courier could not find a customer)
        public static DeliveryStatus Failed = new DeliveryStatus(6, nameof(Failed).ToLowerInvariant());

        public DeliveryStatus(int id, string name)
            : base(id, name)
        { }

        public static IEnumerable<DeliveryStatus> List() =>
            new[] { Planned, CourierAssigned, CourierDeparted, CourierPickedUp, CourierArrived, Failed };

        public static DeliveryStatus FromName(string name)
        {
            var state = List().SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new OrderingDomainException($"Possible values for DeliveryStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static DeliveryStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new OrderingDomainException($"Possible values for DeliveryStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
