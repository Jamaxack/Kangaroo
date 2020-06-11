using Courier.API.Validators;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Courier.UnitTests.ValidatorTests
{
    public class AssignCourierToDeliveryDTOValidatorTests
    {
        readonly AssignCourierToDeliveryDTOValidator _validator;

        public AssignCourierToDeliveryDTOValidatorTests()
        {
            _validator = new AssignCourierToDeliveryDTOValidator();
        }

        #region CourierId
        [Fact]
        public void Shoule_have_error_when_CourierId_is_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.CourierId, Guid.Empty);
        }

        [Fact]
        public void Shoule_not_have_error_when_CourierId_is_specified()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.CourierId, Guid.NewGuid());
        }
        #endregion


        #region DelivertId
        [Fact]
        public void Shoule_have_error_when_DelivertId_is_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.DelivertId, Guid.Empty);
        }

        [Fact]
        public void Shoule_not_have_error_when_DelivertId_is_specified()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.DelivertId, Guid.NewGuid());
        }
        #endregion
    }
}
