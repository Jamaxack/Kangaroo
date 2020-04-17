using Order.Domain.Common;
using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
    public class OrderStatus : Enumeration
    {
        //Newly created order, waiting for verification from our dispatchers
        public static OrderStatus New = new OrderStatus(1, nameof(New).ToLowerInvariant());
        //Order was verified and is available for couriers
        public static OrderStatus Available = new OrderStatus(2, nameof(Available).ToLowerInvariant());
        //A courier was assigned and is working on the order
        public static OrderStatus Active = new OrderStatus(3, nameof(Active).ToLowerInvariant());
        //Order is completed
        public static OrderStatus Completed = new OrderStatus(4, nameof(Completed).ToLowerInvariant());
        //Order was reactivated and is again available for couriers
        public static OrderStatus Reactivated = new OrderStatus(5, nameof(Reactivated).ToLowerInvariant());
        //Order was canceled
        public static OrderStatus Canceled = new OrderStatus(6, nameof(Canceled).ToLowerInvariant());
        //Order execution was delayed by a dispatcher
        public static OrderStatus Delayed = new OrderStatus(7, nameof(Delayed).ToLowerInvariant());
        //Delivery failed (Courier could not find a customer)
        public static OrderStatus Failed = new OrderStatus(8, nameof(Failed).ToLowerInvariant());

        public OrderStatus(short id, string name)
            : base(id, name)
        { }

        public static IEnumerable<OrderStatus> List() =>
            new[] { New, Available, Active, Completed, Reactivated, Canceled, Delayed, Failed };

        public static OrderStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new OrderingDomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static OrderStatus From(short id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new OrderingDomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
