namespace Courier.API.Model
{
    public enum DeliveryStatus
    {
        NotStarted = 1, //Awaiting delivery
        CourierDeparted = 2,
        CourierPickedUp = 3,
        CourierArrived = 4,
        Completed = 5,
        Canceled = 6,
        Failed = 7
    }
}