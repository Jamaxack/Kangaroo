using Delivery.API.Application.Commands;
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
