using FluentValidation;
using Order.API.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Application.Validations
{
    public class SetAvailableDeliveryOrderStatusCommandValidator : AbstractValidator<SetAvailableDeliveryOrderStatusCommand>
    {
        public SetAvailableDeliveryOrderStatusCommandValidator()
        {
            RuleFor(x => x.DeliveryOrderId).NotEmpty();
        }
    }
}
