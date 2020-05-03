using DeliveryOrder.API.Application.Commands;
using FluentValidation;

namespace DeliveryOrder.API.Application.Validations
{
    public class DeleteDeliveryOrderCommandValidator : AbstractValidator<DeleteDeliveryOrderCommand>
    {
        public DeleteDeliveryOrderCommandValidator()
        {
            RuleFor(x => x.DeliveryOrderId).NotEmpty();
        }
    }
}
