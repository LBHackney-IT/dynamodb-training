using AutoFixture;
using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Domain;
using DynamodbTraining.V1.Factories;
using DynamodbTraining.V1.Gateways;
using DynamodbTraining.V1.UseCase;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DynamodbTraining.Tests.V1.UseCase
{
    public class UpdatePersonUseCaseTests
    {
        private readonly Mock<IExampleGateway> _mockGateway;
        private readonly ResponseFactory _responseFactory;
        private readonly UpdatePersonUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        public UpdatePersonUseCaseTests()
        {
            _mockGateway = new Mock<IExampleGateway>();
            _responseFactory = new ResponseFactory();
            _classUnderTest = new UpdatePersonUseCase(_mockGateway.Object, _responseFactory);
        }

        private PersonQueryObject ConstructQuery(Guid id)
        {
            return new PersonQueryObject() { Id = id };
        }

        private PersonRequestObject ConstructRequest()
        {
            return new PersonRequestObject();
        }

        [Fact]
        public async Task UpdatePersonByIdUseCaseGatewayReturnsFound()
        {
            // Arrange
            var request = ConstructRequest();
            var query = ConstructQuery(Guid.NewGuid());
            var person = _fixture.Create<Entity>();
            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(request, query)).ReturnsAsync(person);

            // Act
            var response = await _classUnderTest.ExecuteAsync(request, query).ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(_responseFactory.ToResponse(person));
        }

        [Fact]
        public void UpdatePersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var personRequest = new PersonRequestObject();
            var query = ConstructQuery(Guid.NewGuid());
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(personRequest, query)).ThrowsAsync(exception);

            // Act
            Func<Task<PersonResponseObject>> func = async () => await _classUnderTest.ExecuteAsync(personRequest, query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);

        }
    }
}
