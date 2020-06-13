using System;

namespace Courier.API.Infrastructure.Exceptions
{
    public class CourierDomainException : Exception
    {
        public CourierDomainException()
        {
        }

        public CourierDomainException(string message)
            : base(message)
        {
        }

        public CourierDomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}