﻿using System;

namespace DeliveryOrder.API.Application.Queries
{
    public class DeliveryLocationViewModel
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string BuildingNumber { get; set; }
        public string EntranceNumber { get; set; }
        public string FloorNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Note { get; set; } // Navigation instruction, etc.
        public decimal BuyoutAmount { get; set; } //Amount that courier bought something for client
        public decimal TakingAmount { get; set; } //Amount that client should pay for courier: Delivery price + Buyout price
        public bool IsPaymentInThisDeliveryLocation { get; set; } //Determines if payment will be in this DeliveryLocation
        public string DeliveryLocationAction { get; set; } // PickUp or DropOff
        public string ContactPersonName { get; set; }//Sender or Recipient 
        public string ContactPersonPhone { get; set; }//Sender or Recipient 
        public DateTime? ArrivalStartDateTime { get; set; }
        public DateTime? ArrivalFinishDateTime { get; set; }
        public DateTime? CourierArrivedDateTime { get; set; }
    }
}