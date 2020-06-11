using Delivery.API.Application.Validations;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Delivery.UnitTests.ValidatorTests
{
    public class DeleteDeliveryCommandValidatorTests
    {
        readonly DeleteDeliveryCommandValidator _validator;

        public DeleteDeliveryCommandValidatorTests()
        {
            _validator = new DeleteDeliveryCommandValidator();
        }

        #region DeliveryId
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
        #endregion
    }
}
