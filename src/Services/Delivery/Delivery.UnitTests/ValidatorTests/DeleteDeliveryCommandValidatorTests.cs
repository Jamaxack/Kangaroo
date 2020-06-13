using System;
using Delivery.API.Application.Validations;
using FluentValidation.TestHelper;
using Xunit;

namespace Delivery.UnitTests.ValidatorTests
{
    public class DeleteDeliveryCommandValidatorTests
    {
        public DeleteDeliveryCommandValidatorTests()
        {
            _validator = new DeleteDeliveryCommandValidator();
        }

        private readonly DeleteDeliveryCommandValidator _validator;

        [Fact]
        public void Should_have_error_when_DeliveryId_is_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.DeliveryId, Guid.Empty);
        }

        [Fact]
        public void Should_not_have_error_when_DeliveryId_is_specified()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.DeliveryId, Guid.NewGuid());
        }
    }
}