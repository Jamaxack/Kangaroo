using AutoMapper;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Exceptions;
using Courier.API.Infrastructure.Repositories;
using Courier.API.Infrastructure.Services;
using Courier.API.IntegrationEvents;
using Courier.API.IntegrationEvents.Events;
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

    public class DeliveryServiceTests
    {
        public DeliveryServiceTests()
        {
            _deliveryRepository = new Mock<IDeliveryRepository>();
            _courierRepository = new Mock<ICourierRepository>();
            _integrationEventPublisher = new Mock<IIntegrationEventPublisher>();
            _mapper = new Mock<IMapper>();
            _service = new DeliveryService(_deliveryRepository.Object, _courierRepository.Object, _mapper.Object, _integrationEventPublisher.Object);
        }

        private readonly Mock<IDeliveryRepository> _deliveryRepository;
        private readonly Mock<ICourierRepository> _courierRepository;
        private readonly Mock<IIntegrationEventPublisher> _integrationEventPublisher;
        private readonly Mock<IMapper> _mapper;
        private readonly DeliveryService _service;

        #region GetAvailableDeliveriesAsync

        [Fact]
        public async void DeliveryService_GetAvailableDeliveriesAsync_should_return_deliveries_dtos()
        {
            // Arrange
            var fakeDeliveries = A.ListOf<Delivery>(3);
            var fakeDtos = A.ListOf<DeliveryDto>(3);

            _deliveryRepository.Setup(x => x.GetAvailableDeliveriesAsync()).ReturnsAsync(fakeDeliveries);
            _mapper.Setup(x => x.Map<List<DeliveryDto>>(fakeDeliveries)).Returns(fakeDtos);

            // Act 
            var deliveryDtos = await _service.GetAvailableDeliveriesAsync();

            // Assert
            _deliveryRepository.Verify(x => x.GetAvailableDeliveriesAsync(), Times.Once());
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

            _deliveryRepository.Setup(x => x.GetDeliveriesByCourierIdAsync(fakeCourierId)).ReturnsAsync(fakeDeliveries);
            _mapper.Setup(x => x.Map<List<DeliveryDto>>(fakeDeliveries)).Returns(fakeDeliveryDtos);

            // Act  
            var deliveryDtos = await _service.GetDeliveriesByCourierIdAsync(fakeCourierId);

            // Assert  
            _deliveryRepository.Verify(x => x.GetDeliveriesByCourierIdAsync(fakeCourierId), Times.Once());
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
            _deliveryRepository.Setup(x => x.GetDeliveryByIdAsync(fakeGuid)).ReturnsAsync(() => null);

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

            _deliveryRepository.Setup(x => x.GetDeliveryByIdAsync(fakeEntity.Id)).ReturnsAsync(fakeEntity);
            _mapper.Setup(x => x.Map<DeliveryDto>(fakeEntity)).Returns(fakeDto);

            // Act  
            var entityDto = await _service.GetDeliveryByIdAsync(fakeEntity.Id);

            // Assert  
            _deliveryRepository.Verify(x => x.GetDeliveryByIdAsync(fakeEntity.Id), Times.Once());
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
            _deliveryRepository.Verify(x => x.InsertDeliveryAsync(delivery), Times.Once);
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
            _deliveryRepository.Verify(x => x.DeleteDeliveryByIdAsync(fakeGuid), Times.Once);
        }

        #endregion

        #region AssignCourierToDeliveryAsync

        [Fact]
        public async void DeliveryService_AssignCourierToDeliveryAsync_throws_exception_when_delivery_with_specified_id_is_not_exists()
        {
            // Arrange 
            var dto = A.New<AssignCourierToDeliveryDtoSave>();
            _deliveryRepository.Setup(x => x.GetDeliveryByIdAsync(dto.DeliveryId)).ReturnsAsync(() => null);

            // Act  
            Func<Task> action = async () => { await _service.AssignCourierToDeliveryAsync(dto); };

            // Assert  
            await action.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Delivery not found with specified Id: {dto.DeliveryId}");
            _deliveryRepository.Verify(x => x.GetDeliveryByIdAsync(dto.DeliveryId), Times.Once());
        }

        [Fact]
        public async void DeliveryService_AssignCourierToDeliveryAsync_throws_exception_when_courier_with_specified_id_is_not_exists()
        {
            // Arrange 
            var dto = A.New<AssignCourierToDeliveryDtoSave>();
            var delivery = A.New<Delivery>();
            _deliveryRepository.Setup(x => x.GetDeliveryByIdAsync(dto.DeliveryId)).ReturnsAsync(() => delivery);
            _courierRepository.Setup(x => x.GetCourierByIdAsync(dto.CourierId)).ReturnsAsync(() => null);

            // Act  
            Func<Task> action = async () => { await _service.AssignCourierToDeliveryAsync(dto); };

            // Assert  
            await action.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Courier not found with specified Id: {dto.CourierId}");
            _deliveryRepository.Verify(x => x.GetDeliveryByIdAsync(dto.DeliveryId), Times.Once());
            _courierRepository.Verify(x => x.GetCourierByIdAsync(dto.CourierId), Times.Once());
        }

        [Fact]
        public async void DeliveryService_AssignCourierToDeliveryAsync_assigns_courier_and_publishes_integration_event()
        {
            // Arrange 
            var delivery = A.New<Delivery>();
            var courier = A.New<Courier>();
            var dto = new AssignCourierToDeliveryDtoSave { CourierId = courier.Id, DeliveryId = delivery.Id };

            _deliveryRepository.Setup(x => x.GetDeliveryByIdAsync(dto.DeliveryId)).ReturnsAsync(() => delivery);
            _courierRepository.Setup(x => x.GetCourierByIdAsync(dto.CourierId)).ReturnsAsync(() => courier);

            // Act  
            Func<Task> action = async () => { await _service.AssignCourierToDeliveryAsync(dto); };

            // Assert  
            await action.Should().NotThrowAsync();
            _deliveryRepository.Verify(x => x.GetDeliveryByIdAsync(dto.DeliveryId), Times.Once());
            _courierRepository.Verify(x => x.GetCourierByIdAsync(dto.CourierId), Times.Once());
            _deliveryRepository.Verify(x => x.AssignCourierToDeliveryAsync(dto.DeliveryId, dto.CourierId), Times.Once());
            _deliveryRepository.Verify(x => x.SetDeliveryStatusAsync(dto.DeliveryId, DeliveryStatus.CourierAssigned), Times.Once());
            _integrationEventPublisher.Verify(x => x.Publish(It.Is<CourierAssignedToDeliveryIntegrationEvent>(
                @event => @event.DeliveryId == dto.DeliveryId && @event.CourierId == dto.CourierId)), Times.Once());
        }

        #endregion

        #region SetDeliveryStatusToCourierPickedUpAsync

        [Fact]
        public async void DeliveryService_SetDeliveryStatusToCourierPickedUpAsync_throws_exception_when_id_is_empty()
        {
            // Arrange
            var fakeGuid = Guid.Empty;

            // Act  
            Func<Task> action = async () => { await _service.SetDeliveryStatusToCourierPickedUpAsync(fakeGuid); };

            // Assert 
            await action.Should().ThrowAsync<CourierDomainException>().WithMessage("Delivery Id is not specified");
        }

        [Fact]
        public async void DeliveryService_SetDeliveryStatusToCourierPickedUpAsync_throws_exception_when_delivery_with_specified_id_is_not_exists()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();
            _deliveryRepository.Setup(x => x.GetDeliveryByIdAsync(fakeGuid)).ReturnsAsync(() => null);

            // Act  
            Func<Task> action = async () => { await _service.SetDeliveryStatusToCourierPickedUpAsync(fakeGuid); };

            // Assert  
            await action.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Delivery not found with specified Id: {fakeGuid}");
            _deliveryRepository.Verify(x => x.GetDeliveryByIdAsync(fakeGuid), Times.Once());
        }

        [Fact]
        public async void DeliveryService_SetDeliveryStatusToCourierPickedUpAsync_assigns_delivery_status_and_publishes_integration_event()
        {
            // Arrange 
            var delivery = A.New<Delivery>();
            _deliveryRepository.Setup(x => x.GetDeliveryByIdAsync(delivery.Id)).ReturnsAsync(() => delivery);

            // Act  
            Func<Task> action = async () => { await _service.SetDeliveryStatusToCourierPickedUpAsync(delivery.Id); };

            // Assert  
            await action.Should().NotThrowAsync();
            _deliveryRepository.Verify(x => x.GetDeliveryByIdAsync(delivery.Id), Times.Once());
            _deliveryRepository.Verify(x => x.SetDeliveryStatusAsync(delivery.Id, DeliveryStatus.CourierPickedUp), Times.Once());
            _integrationEventPublisher.Verify(x => x.Publish(It.Is<DeliveryStatusChangedToCourierPickedUpIntegrationEvent>(
                @event => @event.DeliveryId == delivery.Id)), Times.Once());
        }

        #endregion
    }
}
