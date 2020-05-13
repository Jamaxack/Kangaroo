namespace Courier.API.Model
{
    public enum DeliveryStatus
    {
        Available = 1,
        CourierAssigned = 2,
        CourierDeparted = 3,
        CourierPickedUp = 4,
        CourierArrived = 5,
        Completed = 6,
        Canceled = 7,
        Delayed = 8,
        Failed = 9
    }
}