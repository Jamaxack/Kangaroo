using Order.Domain.AggregatesModel.ClientAggregate;
using Order.Domain.Common;
using System;
using System.Collections.Generic;

namespace Order.Domain.AggregatesModel.OrderAggregate
{
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

        public DateTime? PickUpStartDateTime { get; set; }
        public DateTime? PickUpFinishDateTime { get; set; }
        public DateTime? DropOffStartDateTime { get; set; }
        public DateTime? DropOffFinishDateTime { get; set; }
        public DateTime? CourierArrivedPickUpDateTime { get; set; }
        public DateTime? CourierArrivedDropOffDateTime { get; set; }
        public ContactPerson ContactPerson { get; set; }//Sender or Recipient 
        public Order Order { get; set; }
        public decimal BuyoutAmount { get; set; } //Amount that courier bought something for client
        public decimal TakingAmount { get; set; } //Amount that client should pay for courier: Delivery price + Buyout price
        public bool IsOrderPaymentHere { get; set; } //Determines if payment will be in this point
        
        public Point() { }

        public Point(string street, string city, string state, string country, string zipcode, string comment)
        {
           /* Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
            Comment = comment;*/
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            /* yield return Street;
             yield return City;
             yield return State;
             yield return Country;
             yield return ZipCode;
             yield return Comment;*/
            return null;
        }
    }
}
