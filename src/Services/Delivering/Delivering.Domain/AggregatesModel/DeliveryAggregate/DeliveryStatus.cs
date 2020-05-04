using Delivering.Domain.Common;
using Delivering.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delivering.Domain.AggregatesModel.DeliveryAggregate
{
    public class DeliveryStatus : Enumeration
    {
        //Newly created Delivery, waiting for verification from our dispatchers
        public static DeliveryStatus New = new DeliveryStatus(1, nameof(New).ToLowerInvariant());
        //Delivery was verified and is available for couriers
        public static DeliveryStatus Available = new DeliveryStatus(2, nameof(Available).ToLowerInvariant());
        //A courier was assigned and is working on the Delivery
        public static DeliveryStatus CourierAssigned = new DeliveryStatus(3, nameof(CourierAssigned).ToLowerInvariant());
        //Courier departed to the pick-up DeliveryLocation
        public static DeliveryStatus CourierDeparted = new DeliveryStatus(4, nameof(CourierDeparted).ToLowerInvariant());
        //Courier took parcel at the pick-up DeliveryLocation
        public static DeliveryStatus CourierPickedUp = new DeliveryStatus(5, nameof(CourierPickedUp).ToLowerInvariant());
        //Courier has arrived and is waiting for a customer
        public static DeliveryStatus CourierArrived = new DeliveryStatus(6, nameof(CourierArrived).ToLowerInvariant());
        //Delivery is completed
        public static DeliveryStatus Completed = new DeliveryStatus(7, nameof(Completed).ToLowerInvariant());
        //Delivery was reactivated and is again available for couriers
        public static DeliveryStatus Reactivated = new DeliveryStatus(8, nameof(Reactivated).ToLowerInvariant());
        //Delivery was canceled
        public static DeliveryStatus Canceled = new DeliveryStatus(9, nameof(Canceled).ToLowerInvariant());
        //Delivery execution was delayed by a dispatcher
        public static DeliveryStatus Delayed = new DeliveryStatus(10, nameof(Delayed).ToLowerInvariant());
        //Delivery failed (Courier could not find a customer)
        public static DeliveryStatus Failed = new DeliveryStatus(11, nameof(Failed).ToLowerInvariant());

        public DeliveryStatus(int id, string name)
            : base(id, name)
        { }

        public static IEnumerable<DeliveryStatus> List() => new[] { New, Available, CourierAssigned, CourierDeparted, CourierPickedUp, CourierArrived, Completed, Reactivated, Canceled, Delayed, Failed };

        public static DeliveryStatus FromName(string name)
        {
            var state = List().SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
                throw new DeliveringDomainException($"Possible values for DeliveryStatus: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }

        public static DeliveryStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
                throw new DeliveringDomainException($"Possible values for DeliveryStatus: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }
    }
}
