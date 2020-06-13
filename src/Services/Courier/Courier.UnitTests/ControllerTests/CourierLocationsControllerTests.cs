using Courier.API.Controllers;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Services;
using FluentAssertions;
using GenFu;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Courier.UnitTests.ControllerTests
{
    public class CourierLocationsControllerTests
    {
        public CourierLocationsControllerTests()
        {
            _service = new Mock<ICourierLocationService>();
            _controller = new CourierLocationsController(_service.Object);
        }

        private readonly Mock<ICourierLocationService> _service;
        private readonly CourierLocationsController _controller;

        #region CreateAsync

        [Fact]
        public async void CourierLocationsController_CreateAsync_should_return_accepted()
        {
            // Arrange
            var dto = A.New<CourierLocationDtoSave>();

            // Act
            var result = await _controller.CreateAsync(dto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            _service.Verify(x => x.InsertCourierLocationAsync(dto), Times.Once());
        }

        #endregion
    }
}
