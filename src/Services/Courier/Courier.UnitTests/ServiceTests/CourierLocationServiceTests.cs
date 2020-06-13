using System;
using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Repositories;
using Courier.API.Infrastructure.Services;
using Courier.API.Model;
using FluentAssertions;
using Kangaroo.Common.Facades;
using Moq;
using Xunit;

namespace Courier.UnitTests.ServiceTests
{
    public class CourierLocationServiceTests
    {
        public CourierLocationServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _repository = new Mock<ICourierLocationRepository>();
            _dateTimeFacade = new Mock<IDateTimeFacade>();
            _service = new CourierLocationService(_repository.Object, _mapper.Object, _dateTimeFacade.Object);
        }

        private readonly Mock<ICourierLocationRepository> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDateTimeFacade> _dateTimeFacade;
        private readonly CourierLocationService _service;

        [Fact]
        public async void CourierLocationService_InsertCourierLocationAsync_should_map_and_insert_document()
        {
            //Arrange
            var utcNow = DateTime.UtcNow;
            var courierLocationDtoSave = GenFu.GenFu.New<CourierLocationDtoSave>();
            var courierLocation = GenFu.GenFu.New<CourierLocation>();

            _mapper.Setup(x => x.Map<CourierLocation>(courierLocationDtoSave)).Returns(courierLocation);
            _dateTimeFacade.Setup(x => x.UtcNow).Returns(utcNow);

            //Act
            await _service.InsertCourierLocationAsync(courierLocationDtoSave);

            //Assert
            _mapper.Verify(x => x.Map<CourierLocation>(courierLocationDtoSave), Times.Once);
            _dateTimeFacade.Verify(x => x.UtcNow, Times.Once);
            courierLocation.DateTime.Should().Be(utcNow);
            _repository.Verify(x => x.InsertCourierLocationAsync(courierLocation), Times.Once);
        }
    }
}