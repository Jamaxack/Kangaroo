using FluentValidation;
using Pricing.API.DataTransferableObjects;

namespace Pricing.API.Validators
{
    public class CalculatePriceDtoValidator : AbstractValidator<CalculatePriceDto>, IFluentValidator
    {
        public CalculatePriceDtoValidator()
        {
            RuleFor(x => x.Weight).Must(weight => weight > 0 && weight <= 20)
                .WithMessage("Weight should more than 0 and less or equal to 20");
            RuleFor(x => x.PickUpLocation).NotNull().SetValidator(new DeliveryLocationDtoValidator());
            RuleFor(x => x.DropOffLocation).NotNull().SetValidator(new DeliveryLocationDtoValidator());
        }
    }
}