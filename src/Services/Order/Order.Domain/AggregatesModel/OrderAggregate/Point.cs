using Order.Domain.AggregatesModel.ClientAggregate;
using Order.Domain.Common;
using System;
using System.Collections.Generic;

namespace Order.Domain.AggregatesModel.OrderAggregate
{

    public enum ActionType
    {
        PickUp,
        DropOff
    }

    public class Point : ValueObject
    {
        public string Note { get; private set; } // Navigation instruction, etc.
        public string Address { get; private set; }
        public string BuildingNumber { get; set; }
        public string EnterenceNumber { get; set; }
        public string FloorNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ActionType ActionType { get; set; } // PickUp or DropOff
        public DateTime? ArrivalStartDateTime { get; set; }
        public DateTime? ArrivalFinishDateTime { get; set; }
        public DateTime? CourierArrivedDateTime { get; set; }
        public ContactPerson ContactPerson { get; set; }//Sender or Recipient 
        public DeliveryOrder Order { get; set; }
        public decimal BuyoutAmount { get; set; } //Amount that courier bought something for client
        public decimal TakingAmount { get; set; } //Amount that client should pay for courier: Delivery price + Buyout price
        public bool IsOrderPaymentHere { get; set; } //Determines if payment will be in this point

        public Point() { }

        public Point(string address)
        {
            //TODO: Assign all properties
            Address = address;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            //TODO: yield return all atomic properties
            yield return Address;
        }
    }
}
