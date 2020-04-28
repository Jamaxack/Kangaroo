using FluentValidation;
using Order.API.Application.Commands;

namespace Order.API.Application.Validations
{
    public class CreateDeliveryOrderCommandValidator : AbstractValidator<CreateDeliveryOrderCommand>
    {
        public CreateDeliveryOrderCommandValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.DeliveryLocations.Count).GreaterThanOrEqualTo(2);//At least pickup and drop off locations
        }
    }
}
