using Delivering.API.Application.Commands;
using FluentValidation;

namespace Delivering.API.Application.Validations
{
    public class DeleteDeliveryOrderCommandValidator : AbstractValidator<DeleteDeliveryOrderCommand>
    {
        public DeleteDeliveryOrderCommandValidator()
        {
            RuleFor(x => x.DeliveryOrderId).NotEmpty();
        }
    }
}
