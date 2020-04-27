using FluentValidation;
using Order.API.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Application.Validations
{
    public class DeleteDeliveryLocationCommandValidator : AbstractValidator<DeleteDeliveryLocationCommand>
    {
        public DeleteDeliveryLocationCommandValidator()
        {
            RuleFor(x => x.DeliveryOrderId).NotEmpty();
            RuleFor(x => x.DeliveryLocationId).NotEmpty();
        }
    }
}
