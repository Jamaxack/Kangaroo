using System;

namespace Pricing.API.Infrastucture.Exceptions
{
    public class PricingDomainException : Exception
    {
        public PricingDomainException()
        {
        }

        public PricingDomainException(string message)
            : base(message)
        {
        }

        public PricingDomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}