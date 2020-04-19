﻿using Order.Domain.Common;
using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Order.Domain.AggregatesModel.DeliveryOrderAggregate
{
    public class DeliveryOrderStatus : Enumeration
    {
        //Newly created order, waiting for verification from our dispatchers
        public static DeliveryOrderStatus New = new DeliveryOrderStatus(1, nameof(New).ToLowerInvariant());
        //Order was verified and is available for couriers
        public static DeliveryOrderStatus Available = new DeliveryOrderStatus(2, nameof(Available).ToLowerInvariant());
        //A courier was assigned and is working on the order
        public static DeliveryOrderStatus CourierAssigned = new DeliveryOrderStatus(3, nameof(CourierAssigned).ToLowerInvariant());
        //Courier departed to the pick-up DeliveryLocation
        public static DeliveryOrderStatus CourierDeparted = new DeliveryOrderStatus(4, nameof(CourierDeparted).ToLowerInvariant());
        //Courier took parcel at the pick-up DeliveryLocation
        public static DeliveryOrderStatus CourierPickedUp = new DeliveryOrderStatus(5, nameof(CourierPickedUp).ToLowerInvariant());
        //Courier has arrived and is waiting for a customer
        public static DeliveryOrderStatus CourierArrived = new DeliveryOrderStatus(6, nameof(CourierArrived).ToLowerInvariant());
        //Order is completed
        public static DeliveryOrderStatus Completed = new DeliveryOrderStatus(7, nameof(Completed).ToLowerInvariant());
        //Order was reactivated and is again available for couriers
        public static DeliveryOrderStatus Reactivated = new DeliveryOrderStatus(8, nameof(Reactivated).ToLowerInvariant());
        //Order was canceled
        public static DeliveryOrderStatus Canceled = new DeliveryOrderStatus(9, nameof(Canceled).ToLowerInvariant());
        //Order execution was delayed by a dispatcher
        public static DeliveryOrderStatus Delayed = new DeliveryOrderStatus(10, nameof(Delayed).ToLowerInvariant());
        //Delivery failed (Courier could not find a customer)
        public static DeliveryOrderStatus Failed = new DeliveryOrderStatus(11, nameof(Failed).ToLowerInvariant());

        public DeliveryOrderStatus(int id, string name)
            : base(id, name)
        { }

        public static IEnumerable<DeliveryOrderStatus> List() => new[] { New, Available, CourierAssigned, CourierDeparted, CourierPickedUp, CourierArrived, Completed, Reactivated, Canceled, Delayed, Failed };

        public static DeliveryOrderStatus FromName(string name)
        {
            var state = List().SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
                throw new OrderDomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }

        public static DeliveryOrderStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
                throw new OrderDomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");

            return state;
        }
    }
}
