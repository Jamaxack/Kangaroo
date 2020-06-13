using FluentValidation.TestHelper;
using Pricing.API.Validators;
using Xunit;

namespace Pricing.UnitTests.ValidatorTests
{
    public class DeliveryLocationDtoValidatorTests
    {
        readonly DeliveryLocationDtoValidator _validator;

        public DeliveryLocationDtoValidatorTests()
        {
            _validator = new DeliveryLocationDtoValidator();
        }

        #region Address
        [Fact]
        public void Should_have_error_when_Address_is_null()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Address, null as string);
        }

        [Fact]
        public void Should_have_error_when_Address_is_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Address, string.Empty);
        }

        [Fact]
        public void Should_not_have_error_when_Address_is_specified()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Address, "Address");
        }
        #endregion
    }
}
