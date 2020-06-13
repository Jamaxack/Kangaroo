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
    public class CouriersControllerTests
    {
        public CouriersControllerTests()
        {
            _service = new Mock<ICourierService>();
            _controller = new CouriersController(_service.Object);
        }

        private readonly Mock<ICourierService> _service;
        private readonly CouriersController _controller;

         
        [Fact]
        public async void CouriersController_GetAsync_should_return_ok()
        {
            // Act
            var result = await _controller.GetAsync() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            _service.Verify(x => x.GetCouriersAsync(), Times.Once());
        }

        [Fact]
        public async void CouriersController_GetByIdAsync_should_return_ok()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();

            // Act
            var result = await _controller.GetByIdAsync(fakeGuid) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            _service.Verify(x => x.GetCourierByIdAsync(fakeGuid), Times.Once());
        }

        [Fact]
        public async void CouriersController_CreateAsync_should_return_accepted()
        {
            // Arrange
            var dto = A.New<CourierDtoSave>();

            // Act
            var result = await _controller.CreateAsync(dto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            _service.Verify(x => x.InsertCourierAsync(dto), Times.Once());
        }

        [Fact]
        public async void CouriersController_UpdateAsync_should_return_accepted()
        {
            // Arrange
            var dto = A.New<CourierDtoSave>();

            // Act
            var result = await _controller.UpdateAsync(dto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            _service.Verify(x => x.UpdateCourierAsync(dto), Times.Once());
        }

        [Fact]
        public async void CouriersController_GetCurrentLocationByCourierIdAsync_should_return_accepted()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();
             
            // Act
            var result = await _controller.GetCurrentLocationByCourierIdAsync(fakeGuid) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            _service.Verify(x => x.GetCurrentCourierLocationByCourierIdAsync(fakeGuid), Times.Once());
        }
    }
}
