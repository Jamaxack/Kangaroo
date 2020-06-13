using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Exceptions;
using Courier.API.Infrastructure.Repositories;
using Courier.API.Infrastructure.Services;
using FluentAssertions;
using GenFu;
using Kangaroo.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Courier.UnitTests.ServiceTests
{
    using API.Model;
    using System.Collections.Generic;
    using System.Linq;

    public class DeliveryServiceTests
    {
        public DeliveryServiceTests()
        {
            _repository = new Mock<IDeliveryRepository>();
            _eventBus = new Mock<IEventBus>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<DeliveryService>>();
            _service = new DeliveryService(_repository.Object, _eventBus.Object, _mapper.Object, _logger.Object);
        }

        private readonly Mock<IDeliveryRepository> _repository;
        private readonly Mock<IEventBus> _eventBus;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<DeliveryService>> _logger;
        private readonly DeliveryService _service;


        #region GetAvailableDeliveriesAsync

        [Fact]
        public async void DeliveryService_GetAvailableDeliveriesAsync_should_return_deliveries_dtos()
        {
            // Arrange
            var fakeDeliveries = A.ListOf<Delivery>(3);
            var fakeDtos = A.ListOf<DeliveryDto>(3);

            _repository.Setup(x => x.GetAvailableDeliveriesAsync()).ReturnsAsync(fakeDeliveries);
            _mapper.Setup(x => x.Map<List<DeliveryDto>>(fakeDeliveries)).Returns(fakeDtos);

            // Act 
            var deliveryDtos = await _service.GetAvailableDeliveriesAsync();

            // Assert
            _repository.Verify(x => x.GetAvailableDeliveriesAsync(), Times.Once());
            _mapper.Verify(x => x.Map<List<DeliveryDto>>(fakeDeliveries), Times.Once());
            deliveryDtos.Count().Should().Be(3);
        }

        #endregion

        #region GetDeliveriesByCourierIdAsync

        [Fact]
        public async void DeliveryService_GetDeliveriesByCourierIdAsync_throws_exception_when_id_is_empty()
        {
            // Arrange
            var fakeGuid = Guid.Empty;

            // Act  
            Func<Task> action = async () => { await _service.GetDeliveriesByCourierIdAsync(fakeGuid); };

            // Assert 
            await action.Should().ThrowAsync<CourierDomainException>().WithMessage("Courier Id is not specified");
        }

        [Fact]
        public async void DeliveryService_GetDeliveriesByCourierIdAsync_should_return_delivery_dtos_when_existing_courier_id_specified()
        {
            // Arrange 
            var fakeCourierId = Guid.NewGuid();
            var fakeDeliveryDtos = A.ListOf<DeliveryDto>(4);
            var fakeDeliveries = A.ListOf<Delivery>(4);

            _repository.Setup(x => x.GetDeliveriesByCourierIdAsync(fakeCourierId)).ReturnsAsync(fakeDeliveries);
            _mapper.Setup(x => x.Map<List<DeliveryDto>>(fakeDeliveries)).Returns(fakeDeliveryDtos);

            // Act  
            var deliveryDtos = await _service.GetDeliveriesByCourierIdAsync(fakeCourierId);

            // Assert  
            _repository.Verify(x => x.GetDeliveriesByCourierIdAsync(fakeCourierId), Times.Once());
            _mapper.Verify(x => x.Map<List<DeliveryDto>>(fakeDeliveries), Times.Once());
            deliveryDtos.Count().Should().Be(4);
        }

        #endregion

        #region GetDeliveryByIdAsync
        [Fact]
        public async void DeliveryService_GetDeliveryByIdAsync_throws_exception_when_id_is_empty()
        {
            // Arrange
            var fakeGuid = Guid.Empty;

            // Act  
            Func<Task> action = async () => { await _service.GetDeliveryByIdAsync(fakeGuid); };

            // Assert 
            await action.Should().ThrowAsync<CourierDomainException>().WithMessage("Delivery Id is not specified");
        }

        [Fact]
        public async void DeliveryService_GetDeliveryByIdAsync_throws_exception_when_delivery_with_specified_id_is_not_exists()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();
            _repository.Setup(x => x.GetDeliveryByIdAsync(fakeGuid)).ReturnsAsync(() => null);

            // Act  
            Func<Task> action = async () => { await _service.GetDeliveryByIdAsync(fakeGuid); };

            // Assert  
            await action.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Delivery not found with specified Id: {fakeGuid}");
        }

        [Fact]
        public async void DeliveryService_GetDeliveryByIdAsync_should_return_delivery_dto_when_existing_delivery_id_specified()
        {
            // Arrange 
            var fakeEntity = A.New<Delivery>();
            var fakeDto = A.New<DeliveryDto>();

            _repository.Setup(x => x.GetDeliveryByIdAsync(fakeEntity.Id)).ReturnsAsync(fakeEntity);
            _mapper.Setup(x => x.Map<DeliveryDto>(fakeEntity)).Returns(fakeDto);

            // Act  
            var entityDto = await _service.GetDeliveryByIdAsync(fakeEntity.Id);

            // Assert  
            _repository.Verify(x => x.GetDeliveryByIdAsync(fakeEntity.Id), Times.Once());
            _mapper.Verify(x => x.Map<DeliveryDto>(fakeEntity), Times.Once());
            entityDto.Should().NotBeNull();
            entityDto.Should().BeEquivalentTo(fakeDto);
        }

        #endregion

        #region InsertDeliveryAsync
        [Fact]
        public async void DeliveryService_InsertDeliveryAsync_should_throw_exception_when_specified_delivery_is_null()
        {
            // Act  
            Func<Task> action = async () => { await _service.InsertDeliveryAsync(null); };

            // Assert 
            await action.Should().ThrowAsync<CourierDomainException>().WithMessage("Delivery is null");
        }

        [Fact]
        public async void DeliveryService_InsertDeliveryAsync_should_map_and_insert_document()
        {
            //Arrange 
            var deliveryDtoSave = A.New<DeliveryDtoSave>();
            var delivery = A.New<Delivery>();

            _mapper.Setup(x => x.Map<Delivery>(deliveryDtoSave)).Returns(delivery);

            //Act
            await _service.InsertDeliveryAsync(deliveryDtoSave);

            //Assert
            _mapper.Verify(x => x.Map<Delivery>(deliveryDtoSave), Times.Once);
            _repository.Verify(x => x.InsertDeliveryAsync(delivery), Times.Once);
        }

        #endregion

        #region DeleteDeliveryByIdAsync

        [Fact]
        public async void DeliveryService_DeleteDeliveryByIdAsync_should_throw_exception_when_specified_delivery_id_is_empty()
        {
            // Act  
            Func<Task> action = async () => { await _service.DeleteDeliveryByIdAsync(Guid.Empty); };

            // Assert 
            await action.Should().ThrowAsync<CourierDomainException>().WithMessage("Delivery Id is not specified");
        }

        [Fact]
        public async void DeliveryService_DeleteDeliveryByIdAsync_should_delete_document()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();

            // Act  
            Func<Task> deleteAction = async () => { await _service.DeleteDeliveryByIdAsync(fakeGuid); };

            // Assert 
            await deleteAction.Should().NotThrowAsync();
            _repository.Verify(x => x.DeleteDeliveryByIdAsync(fakeGuid), Times.Once);
        }

        #endregion
    }
}
