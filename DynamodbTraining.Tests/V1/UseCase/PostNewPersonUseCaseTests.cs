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
    public class PostNewPersonUseCaseTests
    {
        private readonly Mock<IExampleGateway> _mockGateway;
        private readonly ResponseFactory _responseFactory;
        private readonly PostNewPersonUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        public PostNewPersonUseCaseTests()
        {
            _mockGateway = new Mock<IExampleGateway>();
            _responseFactory = new ResponseFactory();
            _classUnderTest = new PostNewPersonUseCase(_mockGateway.Object, _responseFactory);
        }

        [Fact]
        public async Task CreatePersonByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var personRequest = new PersonRequestObject();

            var person = _fixture.Create<Entity>();

            _mockGateway.Setup(x => x.PostNewPersonAsync(personRequest)).ReturnsAsync(person);

            // Act
            var response = await _classUnderTest.ExecuteAsync(personRequest)
                .ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(_responseFactory.ToResponse(person));
        }

        [Fact]
        public void CreatePersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var personRequest = new PersonRequestObject();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.PostNewPersonAsync(personRequest)).ThrowsAsync(exception);

            // Act
            Func<Task<PersonResponseObject>> func = async () => await _classUnderTest.ExecuteAsync(personRequest).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
