using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Exceptions;
using Courier.API.Infrastructure.Repositories;
using Courier.API.Infrastructure.Services;
using FluentAssertions;
using GenFu;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Courier.UnitTests.ServiceTests
{
    using API.Model;

    public class CourierServiceTests
    {
        public CourierServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _repository = new Mock<ICourierRepository>();
            _service = new CourierService(_repository.Object, _mapper.Object);
        }

        private readonly Mock<ICourierRepository> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly CourierService _service;

        #region GetCourierByIdAsync

        [Fact]
        public async void CourierService_GetCourierByIdAsync_throws_exception_when_id_is_empty()
        {
            // Arrange
            var fakeGuid = Guid.Empty;

            // Act  
            Func<Task> action = async () => { await _service.GetCourierByIdAsync(fakeGuid); };

            // Assert 
            await action.Should().ThrowAsync<CourierDomainException>().WithMessage("Courier Id is not specified");
        }

        [Fact]
        public async void CourierService_GetCourierByIdAsync_throws_exception_when_courier_with_specified_id_is_not_exists()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();
            _repository.Setup(x => x.GetCourierByIdAsync(fakeGuid)).ReturnsAsync(() => null);

            // Act  
            Func<Task> action = async () => { await _service.GetCourierByIdAsync(fakeGuid); };

            // Assert  
            await action.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Courier not found with specified Id: {fakeGuid}");
        }

        [Fact]
        public async void CourierService_GetCourierByIdAsync_should_return_courier_dto_when_existing_courier_id_specified()
        {
            // Arrange 
            var fakeEntity = A.New<Courier>();
            var fakeDto = A.New<CourierDto>();

            _repository.Setup(x => x.GetCourierByIdAsync(fakeEntity.Id)).ReturnsAsync(fakeEntity);
            _mapper.Setup(x => x.Map<CourierDto>(fakeEntity)).Returns(fakeDto);

            // Act  
            var entityDto = await _service.GetCourierByIdAsync(fakeEntity.Id);

            // Assert  
            _repository.Verify(x => x.GetCourierByIdAsync(fakeEntity.Id), Times.Once());
            _mapper.Verify(x => x.Map<CourierDto>(fakeEntity), Times.Once());
            entityDto.Should().NotBeNull();
            entityDto.Should().BeEquivalentTo(fakeDto);
        }

        #endregion

        #region GetCouriersAsync

        [Fact]
        public async void CourierService_GetCouriersAsync_should_return_courier_dtos()
        {
            // Arrange
            var fakeCouriers = A.ListOf<Courier>(3);
            var fakeDtos = A.ListOf<CourierDto>(3);

            _repository.Setup(x => x.GetCouriersAsync()).ReturnsAsync(fakeCouriers);
            _mapper.Setup(x => x.Map<List<CourierDto>>(fakeCouriers)).Returns(fakeDtos);

            // Act 
            var courierDtos = await _service.GetCouriersAsync();

            // Assert
            _repository.Verify(x => x.GetCouriersAsync(), Times.Once());
            _mapper.Verify(x => x.Map<List<CourierDto>>(fakeCouriers), Times.Once());
            courierDtos.Count().Should().Be(3);
        }

        #endregion

        #region GetCurrentCourierLocationByCourierIdAsync

        [Fact]
        public async void CourierService_GetCurrentCourierLocationByCourierIdAsync_throws_exception_when_id_is_empty()
        {
            // Arrange
            var fakeGuid = Guid.Empty;

            // Act  
            Func<Task> action = async () => { await _service.GetCurrentCourierLocationByCourierIdAsync(fakeGuid); };

            // Assert 
            await action.Should().ThrowAsync<CourierDomainException>().WithMessage("Courier Id is not specified");
        }

        [Fact]
        public async void CourierService_GetCurrentCourierLocationByCourierIdAsync_should_return_courier_location_dto_when_existing_courier_id_specified()
        {
            // Arrange 
            var courierId = Guid.NewGuid();
            var fakeLocation = A.New<CourierLocation>();
            var fakeLocationDto = A.New<CourierLocationDto>();

            _repository.Setup(x => x.GetCurrentCourierLocationByCourierIdAsync(courierId)).ReturnsAsync(fakeLocation);
            _mapper.Setup(x => x.Map<CourierLocationDto>(fakeLocation)).Returns(fakeLocationDto);

            // Act  
            var entityDto = await _service.GetCurrentCourierLocationByCourierIdAsync(courierId);

            // Assert  
            _repository.Verify(x => x.GetCurrentCourierLocationByCourierIdAsync(courierId), Times.Once());
            _mapper.Verify(x => x.Map<CourierLocationDto>(fakeLocation), Times.Once());
            entityDto.Should().NotBeNull();
            entityDto.Should().BeEquivalentTo(fakeLocationDto);
        }

        #endregion

        #region InsertCourierAsync

        [Fact]
        public async void CourierService_InsertCourierAsync_should_throw_exception_when_specified_courier_is_null()
        {
            // Act  
            Func<Task> action = async () => { await _service.InsertCourierAsync(null); };

            // Assert 
            await action.Should().ThrowAsync<CourierDomainException>().WithMessage("Courier is null");
        }

        [Fact]
        public async void CourierService_InsertCourierAsync_should_map_and_insert_document()
        {
            //Arrange 
            var courierDtoSave = A.New<CourierDtoSave>();
            var courier = A.New<Courier>();

            _mapper.Setup(x => x.Map<Courier>(courierDtoSave)).Returns(courier); 

            //Act
            await _service.InsertCourierAsync(courierDtoSave);

            //Assert
            _mapper.Verify(x => x.Map<Courier>(courierDtoSave), Times.Once);  
            _repository.Verify(x => x.InsertCourierAsync(courier), Times.Once);
        }

        #endregion

        #region UpdateCourierAsync

        [Fact]
        public async void CourierService_UpdateCourierAsync_should_throw_exception_when_specified_courier_is_null()
        {
            // Act  
            Func<Task> action = async () => { await _service.UpdateCourierAsync(null); };

            // Assert 
            await action.Should().ThrowAsync<CourierDomainException>().WithMessage("Courier is null");
        }

        [Fact]
        public async void CourierService_UpdateCourierAsync_should_map_and_update_document()
        {
            //Arrange 
            var courierDtoSave = A.New<CourierDtoSave>();
            var courier = A.New<Courier>();

            _mapper.Setup(x => x.Map<Courier>(courierDtoSave)).Returns(courier);

            //Act
            await _service.UpdateCourierAsync(courierDtoSave);

            //Assert
            _mapper.Verify(x => x.Map<Courier>(courierDtoSave), Times.Once);
            _repository.Verify(x => x.UpdateCourierAsync(courier), Times.Once);
        }

        #endregion
    }
}
