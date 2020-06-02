using FluentValidation;
using Pricing.API.DataTransferableObjects;

namespace Pricing.API.Validators
{
    public class CalculatePriceDTOValidator : AbstractValidator<CalculatePriceDTO>, IFluentValidator
    {
        public CalculatePriceDTOValidator()
        {
            RuleFor(x => x.Weight).Must(weight => weight > 0 && weight <= 20).WithMessage("Weight should more than 0 and less or equal to 20");
            RuleFor(x => x.PickUpLocation).NotNull().SetValidator(new DeliveryLocationDTOValidator());
            RuleFor(x => x.DropOffLocation).NotNull().SetValidator(new DeliveryLocationDTOValidator());
        }
    }
}
