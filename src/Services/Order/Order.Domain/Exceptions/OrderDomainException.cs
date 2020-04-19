using System;

namespace Order.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class OrderDomainException : Exception
    {
        public OrderDomainException()
        { }

        public OrderDomainException(string message)
            : base(message)
        { }

        public OrderDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
