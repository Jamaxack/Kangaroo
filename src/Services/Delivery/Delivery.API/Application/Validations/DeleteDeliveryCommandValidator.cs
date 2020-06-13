using Delivery.API.Application.Commands.DeliveryAggregate;
using FluentValidation;

namespace Delivery.API.Application.Validations
{
    public class DeleteDeliveryCommandValidator : AbstractValidator<DeleteDeliveryCommand>
    {
        public DeleteDeliveryCommandValidator()
        {
            RuleFor(x => x.DeliveryId).NotEmpty();
        }
    }
}
