using Delivery.API.Application.Commands;
using Delivery.API.Application.Commands.DataTransferableObjects;
using Delivery.API.Application.Commands.DeliveryAggregate;
using Delivery.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Delivery.UnitTests.ApplicationTests
{
    public class IdentifiedCommandHandlerTest
    {
        private readonly Mock<IRequestManager> _requestManager;
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<IdentifiedCommandHandler<CreateDeliveryCommand, bool>>> _logger;

        public IdentifiedCommandHandlerTest()
        {
            _requestManager = new Mock<IRequestManager>();
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<IdentifiedCommandHandler<CreateDeliveryCommand, bool>>>();
        }

        [Fact]
        public async Task Handler_sends_command_when_delivery_not_exists()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();
            var fakeDeliveryCommand = new IdentifiedCommand<CreateDeliveryCommand, bool>(FakeDeliveryRequest(), fakeGuid);

            _requestManager.Setup(x => x.ExistAsync(It.IsAny<Guid>()))
               .Returns(Task.FromResult(false));

            _mediator.Setup(x => x.Send(It.IsAny<IRequest<bool>>(), default))
               .Returns(Task.FromResult(true));

            // Act
            var handler = new IdentifiedCommandHandler<CreateDeliveryCommand, bool>(_mediator.Object, _requestManager.Object, _logger.Object);
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(fakeDeliveryCommand, cancellationToken);

            // Assert
            Assert.True(result);
            _mediator.Verify(x => x.Send(It.IsAny<IRequest<bool>>(), default), Times.Once());
        }

        [Fact]
        public async Task Handler_should_not_send_command_when_delivery_already_exists()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();
            var fakeDeliveryCommand = new IdentifiedCommand<CreateDeliveryCommand, bool>(FakeDeliveryRequest(), fakeGuid);

            _requestManager.Setup(x => x.ExistAsync(It.IsAny<Guid>()))
               .Returns(Task.FromResult(true));

            // Act
            var handler = new IdentifiedCommandHandler<CreateDeliveryCommand, bool>(_mediator.Object, _requestManager.Object, _logger.Object);
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(fakeDeliveryCommand, cancellationToken);

            // Assert
            Assert.False(result);
            _mediator.Verify(x => x.Send(It.IsAny<IRequest<bool>>(), default), Times.Never());
        }

        private CreateDeliveryCommand FakeDeliveryRequest()
        {
            var pickUpLocation = new DeliveryLocationDto() { Address = "PickUpAddress" };
            var dropOffLocation = new DeliveryLocationDto() { Address = "DropOffAddress" };
            return new CreateDeliveryCommand
            {
                ClientId = Guid.NewGuid(),
                Price = 7,
                Weight = 3,
                Note = "Some important note",
                PickUpLocation = pickUpLocation,
                DropOffLocation = dropOffLocation
            };
        }
    }
}
