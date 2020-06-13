using System;
using Courier.API.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Courier.UnitTests.ValidatorTests
{
    public class CourierLocationDtoSaveValidatorTests
    {
        public CourierLocationDtoSaveValidatorTests()
        {
            _validator = new CourierLocationDtoSaveValidator();
        }

        private readonly CourierLocationDtoSaveValidator _validator;

        [Theory]
        [InlineData(-120)]
        [InlineData(-91)]
        [InlineData(91)]
        [InlineData(98)]
        public void Should_have_error_when_Latitude_is_not_between_negative_90_and_90(int latitude)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Latitude, latitude)
                .WithErrorMessage("Latitude must be between -90 and 90 degrees inclusive.");
        }

        [Theory]
        [InlineData(-90)]
        [InlineData(-82)]
        [InlineData(57)]
        [InlineData(90)]
        public void Should_not_have_error_when_Latitude_is_between_negative_90_and_90_inclusive(int latitude)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Latitude, latitude);
        }

        [Theory]
        [InlineData(-218)]
        [InlineData(-181)]
        [InlineData(188)]
        [InlineData(245)]
        public void Should_have_error_when_Longitude_is_not_between_negative_180_and_180(int longitude)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Longitude, longitude)
                .WithErrorMessage("Longitude must be between -180 and 180 degrees inclusive.");
        }

        [Theory]
        [InlineData(-180)]
        [InlineData(-82)]
        [InlineData(57)]
        [InlineData(180)]
        public void Should_not_have_error_when_Longitude_is_between_negative_180_and_180_inclusive(int longitude)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Longitude, longitude);
        }

        [Fact]
        public void Should_have_error_when_CourierId_is_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.CourierId, Guid.Empty);
        }

        [Fact]
        public void Should_not_have_error_when_CourierId_is_specified()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.CourierId, Guid.NewGuid());
        }
    }
}