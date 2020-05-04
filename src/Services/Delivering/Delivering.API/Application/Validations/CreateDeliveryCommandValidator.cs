using FluentValidation;
using Delivering.API.Application.Commands;

namespace Delivering.API.Application.Validations
{
    public class CreateDeliveryCommandValidator : AbstractValidator<CreateDeliveryCommand>
    {
        public CreateDeliveryCommandValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.PickUpLocation).NotNull();
            RuleFor(x => x.DropOffLocation).NotNull();
        }
    }
}
