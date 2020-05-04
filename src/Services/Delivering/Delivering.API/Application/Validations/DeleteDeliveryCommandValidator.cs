using Delivering.API.Application.Commands;
using FluentValidation;

namespace Delivering.API.Application.Validations
{
    public class DeleteDeliveryCommandValidator : AbstractValidator<DeleteDeliveryCommand>
    {
        public DeleteDeliveryCommandValidator()
        {
            RuleFor(x => x.DeliveryId).NotEmpty();
        }
    }
}
