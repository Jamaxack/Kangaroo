using Delivery.API.Application.Commands.DataTransferableObjects;
using Delivery.API.Application.Commands.DeliveryAggregate;
using FluentAssertions;
using GenFu;
using Kangaroo.Common.Facades;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Delivery.UnitTests.ApplicationTests
{
    using Domain.AggregatesModel.DeliveryAggregate;

    public class CreateDeliveryCommandHandlerTest
    {
        private readonly Mock<IDeliveryRepository> _deliveryRepository;
        private readonly Mock<ILogger<CreateDeliveryCommandHandler>> _logger;
        private readonly Mock<IDateTimeFacade> _dateTimeFacade;


        public CreateDeliveryCommandHandlerTest()
        {
            _deliveryRepository = new Mock<IDeliveryRepository>();
            _logger = new Mock<ILogger<CreateDeliveryCommandHandler>>();
            _dateTimeFacade = new Mock<IDateTimeFacade>();
        }


        [Fact]
        public async Task Handle_return_false_if_order_is_not_persisted()
        {
            var now = DateTime.UtcNow;
            _dateTimeFacade.Setup(x => x.UtcNow).Returns(now);

            _deliveryRepository.Setup(repository => repository.GetAsync(It.IsAny<Guid>()))
               .Returns(Task.FromResult<Delivery>(BuildDelivery(_dateTimeFacade.Object)));

            _deliveryRepository.Setup(repository => repository.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(false));

            //Act
            var handler = new CreateDeliveryCommandHandler(_deliveryRepository.Object, _logger.Object, _dateTimeFacade.Object);
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(FakeDeliveryCommand(), cancellationToken);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_return_true_if_order_is_persisted_successfully()
        {
            var now = DateTime.UtcNow;
            _dateTimeFacade.Setup(x => x.UtcNow).Returns(now);

            _deliveryRepository.Setup(repository => repository.GetAsync(It.IsAny<Guid>()))
               .Returns(Task.FromResult<Delivery>(BuildDelivery(_dateTimeFacade.Object)));

            _deliveryRepository.Setup(repository => repository.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            //Act
            var handler = new CreateDeliveryCommandHandler(_deliveryRepository.Object, _logger.Object, _dateTimeFacade.Object);
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(FakeDeliveryCommand(), cancellationToken);

            //Assert
            result.Should().BeTrue();
        }

        private Delivery BuildDelivery(IDateTimeFacade dateTimeFacade)
            => new Delivery(
                clientId: Guid.NewGuid(),
                price: 5,
                weight: 2,
                note: "Important note",
                createdDateTime: dateTimeFacade.UtcNow
                );

        private CreateDeliveryCommand FakeDeliveryCommand()
        {
            var fakeLocation = A.New<DeliveryLocationDto>();
            fakeLocation.ContactPerson = A.New<ContactPersonDto>();
            var fakeCommand = A.New<CreateDeliveryCommand>();
            fakeCommand.PickUpLocation = fakeLocation;
            fakeCommand.DropOffLocation = fakeLocation;
            return fakeCommand;
        }
    }
}
