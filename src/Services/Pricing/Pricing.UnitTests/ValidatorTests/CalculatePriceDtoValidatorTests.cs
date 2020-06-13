using FluentValidation.TestHelper;
using Pricing.API.DataTransferableObjects;
using Pricing.API.Validators;
using Xunit;

namespace Pricing.UnitTests.ValidatorTests
{
    public class CalculatePriceDtoValidatorTests
    {
        public CalculatePriceDtoValidatorTests()
        {
            _validator = new CalculatePriceDtoValidator();
        }

        private readonly CalculatePriceDtoValidator _validator;

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(21)]
        public void Should_have_error_when_Weight_is_not_greater_than_0_and_less_or_equal_20(short weight)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Weight, weight)
                .WithErrorMessage("Weight should more than 0 and less or equal to 20");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(19)]
        [InlineData(20)]
        public void Should_not_have_error_when_Weight_is_greater_than_0_and_less_or_equal_20(short weight)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Weight, weight);
        }

        [Fact]
        public void Should_have_error_when_DropOffLocation_is_null()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.DropOffLocation, null as DeliveryLocationDto);
        }

        [Fact]
        public void Should_have_error_when_PickUpLocation_is_null()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.PickUpLocation, null as DeliveryLocationDto);
        }

        [Fact]
        public void Should_not_have_error_when_DropOffLocation_is_specified()
        {
            var deliveryLocation = GenFu.GenFu.New<DeliveryLocationDto>();
            _validator.ShouldNotHaveValidationErrorFor(x => x.DropOffLocation, deliveryLocation);
        }

        [Fact]
        public void Should_not_have_error_when_PickUpLocation_is_specified()
        {
            var deliveryLocation = GenFu.GenFu.New<DeliveryLocationDto>();
            _validator.ShouldNotHaveValidationErrorFor(x => x.PickUpLocation, deliveryLocation);
        }
    }
}