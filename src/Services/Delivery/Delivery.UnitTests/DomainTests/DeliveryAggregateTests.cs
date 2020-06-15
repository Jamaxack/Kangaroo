using Delivery.Domain.Exceptions;
using Xunit;
using FluentAssertions;
using System;

namespace Delivery.UnitTests.DomainTests
{
    using Delivery.Domain.AggregatesModel.DeliveryAggregate;

    public class DeliveryAggregateTests
    {
        [Fact]
        public void Invalid_weight_should_throw_exception()
        {
            //Arrange   
            var clientId = Guid.NewGuid();
            var price = 5;
            short weight = -1;
            var note = "Important note";
            var createdDateTime = DateTime.UtcNow;

            // Act
            Action action = () => new Delivery(clientId, price, weight, note, createdDateTime);

            // Assert
            action.Should().Throw<DeliveryDomainException>().WithMessage("Invalid Weight");
        }
    }
}
