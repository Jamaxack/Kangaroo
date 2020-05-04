using FluentValidation;
using Delivering.API.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivering.API.Application.Validations
{
    public class SetAvailableDeliveryStatusCommandValidator : AbstractValidator<SetAvailableDeliveryStatusCommand>
    {
        public SetAvailableDeliveryStatusCommandValidator()
        {
            RuleFor(x => x.DeliveryId).NotEmpty();
        }
    }
}
