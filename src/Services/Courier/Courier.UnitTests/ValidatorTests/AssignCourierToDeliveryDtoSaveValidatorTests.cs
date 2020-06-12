using Courier.API.Validators;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Courier.UnitTests.ValidatorTests
{
    public class AssignCourierToDeliveryDtoSaveValidatorTests
    {
        readonly AssignCourierToDeliveryDtoSaveValidator _saveValidator;

        public AssignCourierToDeliveryDtoSaveValidatorTests()
        {
            _saveValidator = new AssignCourierToDeliveryDtoSaveValidator();
        }

        #region CourierId

        [Fact]
        public void Should_have_error_when_CourierId_is_empty()
        {
            _saveValidator.ShouldHaveValidationErrorFor(x => x.CourierId, Guid.Empty);
        }

        [Fact]
        public void Should_not_have_error_when_CourierId_is_specified()
        {
            _saveValidator.ShouldNotHaveValidationErrorFor(x => x.CourierId, Guid.NewGuid());
        }

        #endregion


        #region DelivertId

        [Fact]
        public void Should_have_error_when_DeliveryId_is_empty()
        {
            _saveValidator.ShouldHaveValidationErrorFor(x => x.DeliveryId, Guid.Empty);
        }

        [Fact]
        public void Should_not_have_error_when_DeliveryId_is_specified()
        {
            _saveValidator.ShouldNotHaveValidationErrorFor(x => x.DeliveryId, Guid.NewGuid());
        }

        #endregion
    }
}