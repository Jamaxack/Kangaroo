using Courier.API.Controllers;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Services;
using FluentAssertions;
using GenFu;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Net;
using Xunit;

namespace Courier.UnitTests.ControllerTests
{
    public class DeliveriesControllerTests
    {
        public DeliveriesControllerTests()
        {
            _service = new Mock<IDeliveryService>();
            _controller = new DeliveriesController(_service.Object);
        }

        private readonly Mock<IDeliveryService> _service;
        private readonly DeliveriesController _controller;

        [Fact]
        public async void DeliveriesController_GetAvailableDeliveriesAsync_should_return_ok()
        {
            // Act
            var result = await _controller.GetAvailableDeliveriesAsync() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            _service.Verify(x => x.GetAvailableDeliveriesAsync(), Times.Once());
        }

        [Fact]
        public async void DeliveriesController_GetDeliveriesByCourierIdAsync_should_return_ok()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();

            // Act
            var result = await _controller.GetDeliveriesByCourierIdAsync(fakeGuid) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            _service.Verify(x => x.GetDeliveriesByCourierIdAsync(fakeGuid), Times.Once());
        }

        [Fact]
        public async void DeliveriesController_CreateDeliveryAsync_should_return_accepted()
        {
            // Arrange
            var dto = A.New<DeliveryDtoSave>();

            // Act
            var result = await _controller.CreateDeliveryAsync(dto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            _service.Verify(x => x.InsertDeliveryAsync(dto), Times.Once());
        }

        [Fact]
        public async void DeliveriesController_AssignCourierToDeliveryAsync_should_return_accepted()
        {
            // Arrange
            var dto = A.New<AssignCourierToDeliveryDtoSave>();

            // Act
            var result = await _controller.AssignCourierToDeliveryAsync(dto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            _service.Verify(x => x.AssignCourierToDeliveryAsync(dto), Times.Once());
        }

        [Fact]
        public async void DeliveriesController_GetDeliveryByIdAsync_should_return_ok()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();

            // Act
            var result = await _controller.GetDeliveryByIdAsync(fakeGuid) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            _service.Verify(x => x.GetDeliveryByIdAsync(fakeGuid), Times.Once());
        }

        [Fact]
        public async void DeliveriesController_DeleteDeliveryByIdAsync_should_return_accepted()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();

            // Act
            var result = await _controller.DeleteDeliveryByIdAsync(fakeGuid) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            _service.Verify(x => x.DeleteDeliveryByIdAsync(fakeGuid), Times.Once());
        }

        [Fact]
        public async void DeliveriesController_PickedUpAsync_should_return_accepted()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();

            // Act
            var result = await _controller.PickedUpAsync(fakeGuid) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            _service.Verify(x => x.SetDeliveryStatusToCourierPickedUpAsync(fakeGuid), Times.Once());
        }
    }
}
