using System;

namespace Delivery.Domain.Exceptions
{
    /// <summary>
    ///     Exception type for domain exceptions
    /// </summary>
    public class DeliveryDomainException : Exception
    {
        public DeliveryDomainException()
        {
        }

        public DeliveryDomainException(string message)
            : base(message)
        {
        }

        public DeliveryDomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}