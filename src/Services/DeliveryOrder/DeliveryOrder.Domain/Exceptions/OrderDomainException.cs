using System;

namespace DeliveryOrder.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class DeliveryOrderDomainException : Exception
    {
        public DeliveryOrderDomainException()
        { }

        public DeliveryOrderDomainException(string message)
            : base(message)
        { }

        public DeliveryOrderDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
