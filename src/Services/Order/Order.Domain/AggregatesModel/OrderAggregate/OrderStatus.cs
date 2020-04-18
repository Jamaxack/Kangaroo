using Order.Domain.Common;
using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class OrderStatus : Enumeration
    {
        //Newly created order, waiting for verification from our dispatchers
        public static OrderStatus New = new OrderStatus(1, nameof(New).ToLowerInvariant());
        //Order was verified and is available for couriers
        public static OrderStatus Available = new OrderStatus(2, nameof(Available).ToLowerInvariant());
        //A courier was assigned and is working on the order
        public static OrderStatus CourierAssigned = new OrderStatus(3, nameof(CourierAssigned).ToLowerInvariant());
        //Courier departed to the pick-up DeliveryLocation
        public static OrderStatus CourierDeparted = new OrderStatus(4, nameof(CourierDeparted).ToLowerInvariant());
        //Courier took parcel at the pick-up DeliveryLocation
        public static OrderStatus CourierPickedUp = new OrderStatus(5, nameof(CourierPickedUp).ToLowerInvariant());
        //Courier has arrived and is waiting for a customer
        public static OrderStatus CourierArrived = new OrderStatus(6, nameof(CourierArrived).ToLowerInvariant());
        //Order is completed
        public static OrderStatus Completed = new OrderStatus(7, nameof(Completed).ToLowerInvariant());
        //Order was reactivated and is again available for couriers
        public static OrderStatus Reactivated = new OrderStatus(8, nameof(Reactivated).ToLowerInvariant());
        //Order was canceled
        public static OrderStatus Canceled = new OrderStatus(9, nameof(Canceled).ToLowerInvariant());
        //Order execution was delayed by a dispatcher
        public static OrderStatus Delayed = new OrderStatus(10, nameof(Delayed).ToLowerInvariant());
        //Delivery failed (Courier could not find a customer)
        public static OrderStatus Failed = new OrderStatus(11, nameof(Failed).ToLowerInvariant());

        public OrderStatus(short id, string name)
            : base(id, name)
        { }

        public static IEnumerable<OrderStatus> List() => new[] { New, Available, CourierAssigned, CourierDeparted, CourierPickedUp, CourierArrived, Completed, Reactivated, Canceled, Delayed, Failed };

        public static OrderStatus FromName(string name)
        {
            var state = List().SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
                throw new OrderingDomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }

        public static OrderStatus From(short id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
                throw new OrderingDomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }
    }
}
