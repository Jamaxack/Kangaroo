using Delivery.API.Application.Commands.DeliveryAggregate;
using FluentValidation;

namespace Delivery.API.Application.Validations
{
    public class SetAvailableDeliveryStatusCommandValidator : AbstractValidator<SetAvailableDeliveryStatusCommand>
    {
        public SetAvailableDeliveryStatusCommandValidator()
        {
            RuleFor(x => x.DeliveryId).NotEmpty();
        }
    }
}