using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Repositories;
using Courier.API.Infrastructure.Services;
using Courier.API.Model;
using FluentAssertions;
using GenFu;
using Kangaroo.Common.Facades;
using Moq;
using System;
using Xunit;

namespace Courier.UnitTests.ServiceTests
{
    public class CourierLocationServiceTests
    {
        readonly Mock<ICourierLocationRepository> _repository;
        readonly Mock<IMapper> _mapper;
        readonly Mock<IDateTimeFacade> _dateTimeFacade;
        readonly CourierLocationService _service;

        public CourierLocationServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _repository = new Mock<ICourierLocationRepository>();
            _dateTimeFacade = new Mock<IDateTimeFacade>();
            _service = new CourierLocationService(_repository.Object, _mapper.Object, _dateTimeFacade.Object);
        }

        #region InsertCourierLocationAsync
        [Fact]
        public async void CourierLocationService_InsertCourierLocationAsync_should_map_and_insert_document()
        {
            //Arrange
            var utcNow = DateTime.UtcNow;
            var courierLocationDtoSave = A.New<CourierLocationDtoSave>();
            var courierLocation = A.New<CourierLocation>();

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
        #endregion

    }
}
