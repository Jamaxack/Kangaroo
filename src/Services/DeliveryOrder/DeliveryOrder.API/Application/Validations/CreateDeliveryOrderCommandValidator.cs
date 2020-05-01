using FluentValidation;
using DeliveryOrder.API.Application.Commands;

namespace DeliveryOrder.API.Application.Validations
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
