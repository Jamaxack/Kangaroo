using FluentValidation;
using Delivery.API.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.API.Application.Validations
{
    public class SetAvailableDeliveryOrderStatusCommandValidator : AbstractValidator<SetAvailableDeliveryOrderStatusCommand>
    {
        public SetAvailableDeliveryOrderStatusCommandValidator()
        {
            RuleFor(x => x.DeliveryOrderId).NotEmpty();
        }
    }
}
