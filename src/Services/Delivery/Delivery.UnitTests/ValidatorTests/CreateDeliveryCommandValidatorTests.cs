using Delivery.API.Application.Validations;
using FluentValidation.TestHelper;
using GenFu;
using System;
using Delivery.API.Application.Commands.DataTransferableObjects;
using Xunit;

namespace Delivery.UnitTests.ValidatorTests
{
    public class CreateDeliveryCommandValidatorTests
    {
        readonly CreateDeliveryCommandValidator _validator;

        public CreateDeliveryCommandValidatorTests()
        {
            _validator = new CreateDeliveryCommandValidator();
        }

        #region ClientId
        [Fact]
        public void Should_have_error_when_ClientId_is_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.ClientId, Guid.Empty);
        }

        [Fact]
        public void Should_not_have_error_when_ClientId_is_specified()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.ClientId, Guid.NewGuid());
        }
        #endregion

        #region PickUpLocation
        [Fact]
        public void Should_have_error_when_PickUpLocation_is_null()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.PickUpLocation, null as DeliveryLocationDto);
        }

        [Fact]
        public void Should_not_have_error_when_PickUpLocation_is_specified()
        {
            var deliveryLocation = A.New<DeliveryLocationDto>();
            _validator.ShouldNotHaveValidationErrorFor(x => x.PickUpLocation, deliveryLocation);
        }
        #endregion

        #region DropOffLocation
        [Fact]
        public void Should_have_error_when_DropOffLocation_is_null()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.DropOffLocation, null as DeliveryLocationDto);
        }

        [Fact]
        public void Should_not_have_error_when_DropOffLocation_is_specified()
        {
            var deliveryLocation = A.New<DeliveryLocationDto>();
            _validator.ShouldNotHaveValidationErrorFor(x => x.DropOffLocation, deliveryLocation);
        }
        #endregion
    }
}
