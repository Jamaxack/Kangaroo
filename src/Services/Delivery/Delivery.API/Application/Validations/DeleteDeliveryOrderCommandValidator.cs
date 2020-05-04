﻿using Delivery.API.Application.Commands;
using FluentValidation;

namespace Delivery.API.Application.Validations
{
    public class DeleteDeliveryOrderCommandValidator : AbstractValidator<DeleteDeliveryOrderCommand>
    {
        public DeleteDeliveryOrderCommandValidator()
        {
            RuleFor(x => x.DeliveryOrderId).NotEmpty();
        }
    }
}