using FluentValidation;
using Pricing.API.DataTransferableObjects;

namespace Pricing.API.Validators
{
    public class DeliveryLocationDTOValidator : AbstractValidator<DeliveryLocationDTO>, IFluentValidator
    {
        public DeliveryLocationDTOValidator()
        {
            RuleFor(x => x.Address).NotEmpty();
        }
    }
}
