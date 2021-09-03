using AutoFixture;
using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Domain;
using DynamodbTraining.V1.Gateways;
using DynamodbTraining.V1.UseCase;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DynamodbTraining.Tests.V1.UseCase
{
    public class GetByIdUseCaseTests
    {
        private Mock<IExampleGateway> _mockGateway;
        private GetByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();


        public GetByIdUseCaseTests()
        {
            _mockGateway = new Mock<IExampleGateway>();
            _classUnderTest = new GetByIdUseCase(_mockGateway.Object);
        }

        private PersonQueryObject ConstructQuery()
        {
            return new PersonQueryObject() { Id = Guid.NewGuid() };
        }

        [Fact]
        public async Task GetByIdUseCaseGatewayReturnsNullReturnsNull()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGateway.Setup(x => x.GetEntityById(query)).ReturnsAsync((Entity) null);

            // Act
            var response = await _classUnderTest.Execute(query).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var query = ConstructQuery();
            var person = _fixture.Create<Entity>();
            _mockGateway.Setup(x => x.GetEntityById(query)).ReturnsAsync(person);

            // Act
            var response = await _classUnderTest.Execute(query).ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(person);
        }

        [Fact]
        public void GetPersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var query = ConstructQuery();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetEntityById(query)).ThrowsAsync(exception);

            // Act
            Func<Task<Entity>> func = async () => await _classUnderTest.Execute(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
