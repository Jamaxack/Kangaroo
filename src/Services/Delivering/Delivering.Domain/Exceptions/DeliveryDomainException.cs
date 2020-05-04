using System;

namespace Delivering.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class DeliveringDomainException : Exception
    {
        public DeliveringDomainException()
        { }

        public DeliveringDomainException(string message)
            : base(message)
        { }

        public DeliveringDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
