using FluentValidation;
using Delivering.API.Application.Commands;

namespace Delivering.API.Application.Validations
{
    public class CreateDeliveryOrderCommandValidator : AbstractValidator<CreateDeliveryOrderCommand>
    {
        public CreateDeliveryOrderCommandValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.PickUpLocation).NotNull();
            RuleFor(x => x.DropOffLocation).NotNull();
        }
    }
}
