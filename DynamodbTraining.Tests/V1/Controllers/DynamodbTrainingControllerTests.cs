using AutoFixture;
using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Controllers;
using DynamodbTraining.V1.Domain;
using DynamodbTraining.V1.Factories;
using DynamodbTraining.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DynamodbTraining.Tests.V1.Controllers
{
    public class DynamodbTrainingControllerTests
    {
        private readonly Mock<IGetByIdUseCase> _mockGetByIdUseCase;
        private readonly Mock<IPostNewPersonUseCase> _mockNewPersonUseCase;
        private readonly Mock<IUpdatePersonUseCase> _mockUpdatePersonUseCase;
        private readonly DynamodbTrainingController _classUnderTest;
        private readonly ResponseFactory _responseFactory;
        private readonly Fixture _fixture = new Fixture();


        public DynamodbTrainingControllerTests()
        {
            _mockGetByIdUseCase = new Mock<IGetByIdUseCase>();
            _mockNewPersonUseCase = new Mock<IPostNewPersonUseCase>();
            _mockUpdatePersonUseCase = new Mock<IUpdatePersonUseCase>();
            _responseFactory = new ResponseFactory();

            _classUnderTest = new DynamodbTrainingController(_mockGetByIdUseCase.Object, _mockNewPersonUseCase.Object, _mockUpdatePersonUseCase.Object);
        }

        private PersonQueryObject ConstructQuery()
        {
            return new PersonQueryObject() { Id = Guid.NewGuid() };
        }

        private PersonRequestObject ConstructRequest()
        {
            return new PersonRequestObject();
        }

        [Fact]
        public async Task GetPersonByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGetByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync((Entity) null);

            // Act
            var response = await _classUnderTest.ViewRecord(query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(query.Id);
        }

        [Fact]
        public async Task GetPersonByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var query = ConstructQuery();
            var personResponse = _fixture.Create<Entity>();
            _mockGetByIdUseCase.Setup(x => x.Execute(query)).ReturnsAsync(personResponse);

            // Act
            var response = await _classUnderTest.ViewRecord(query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult).Value.Should().BeEquivalentTo(personResponse);
        }

        [Fact]
        public void GetPersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var query = ConstructQuery();
            var exception = new ApplicationException("Test exception");
            _mockGetByIdUseCase.Setup(x => x.Execute(query)).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _classUnderTest.ViewRecord(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        [Fact]
        public async Task PostNewPersonIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var personResponse = _fixture.Create<PersonResponseObject>();
            _mockNewPersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PersonRequestObject>()))
                .ReturnsAsync(personResponse);

            // Act
            var response = await _classUnderTest.PostNewPerson(new PersonRequestObject()).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(CreatedResult));
            (response as CreatedResult).Value.Should().Be(personResponse);
        }

        [Fact]
        public void PostNewPersonIdAsyncExceptionIsThrown()
        {
            // Arrange
            var exception = new ApplicationException("Test exception");
            _mockNewPersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PersonRequestObject>()))
                                 .ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _classUnderTest.PostNewPerson(new PersonRequestObject())
                .ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        [Fact]
        public async Task UpdatePersonByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var query = ConstructQuery();
            var request = ConstructRequest();
            _mockUpdatePersonUseCase.Setup(x => x.ExecuteAsync(request, query)).ReturnsAsync((PersonResponseObject) null);

            // Act
            var response = await _classUnderTest.UpdatePersonByIdAsync(request, query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(query.Id);
        }

        [Fact]
        public async Task UpdatePersonByIdAsyncFoundReturnsFound()
        {
            // Arrange
            var query = ConstructQuery();
            var request = ConstructRequest();
            var personResponse = _fixture.Create<PersonResponseObject>();
            _mockUpdatePersonUseCase.Setup(x => x.ExecuteAsync(request, query)).ReturnsAsync(personResponse);

            // Act
            var response = await _classUnderTest.UpdatePersonByIdAsync(request, query).ConfigureAwait(false);
            // Assert
            response.Should().BeOfType(typeof(NoContentResult));

        }

        [Fact]
        public void UpdatePersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var query = ConstructQuery();
            var exception = new ApplicationException("Test exception");
            _mockUpdatePersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PersonRequestObject>(), query)).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _classUnderTest.UpdatePersonByIdAsync(new PersonRequestObject(), query)
                .ConfigureAwait(false);
            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
