using System;
using Delivery.API.Application.Commands.DataTransferableObjects;
using Delivery.API.Application.Validations;
using FluentValidation.TestHelper;
using Xunit;

namespace Delivery.UnitTests.ValidatorTests
{
    public class CreateDeliveryCommandValidatorTests
    {
        public CreateDeliveryCommandValidatorTests()
        {
            _validator = new CreateDeliveryCommandValidator();
        }

        private readonly CreateDeliveryCommandValidator _validator;

        [Fact]
        public void Should_have_error_when_ClientId_is_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.ClientId, Guid.Empty);
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
        public void Should_not_have_error_when_ClientId_is_specified()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.ClientId, Guid.NewGuid());
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