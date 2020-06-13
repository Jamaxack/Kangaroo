using FluentValidation;
using Pricing.API.DataTransferableObjects;

namespace Pricing.API.Validators
{
    public class DeliveryLocationDtoValidator : AbstractValidator<DeliveryLocationDto>, IFluentValidator
    {
        public DeliveryLocationDtoValidator()
        {
            RuleFor(x => x.Address).NotEmpty();
        }
    }
}
