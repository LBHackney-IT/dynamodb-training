using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Domain;
using DynamodbTraining.V1.Factories;
using DynamodbTraining.V1.Gateways;
using DynamodbTraining.V1.Infrastructure;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DynamodbTraining.Tests.V1.Gateways
{
    [Collection("DynamoDb collection")]

    public class DynamoDbGatewayTests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        private DynamoDbGateway _classUnderTest;
        private readonly IDynamoDBContext _dynamoDb;

        private readonly List<Action> _cleanup = new List<Action>();


        public DynamoDbGatewayTests(DynamoDbIntegrationTests<Startup> dbTestFixture)
        {
            _dynamoDb = dbTestFixture.DynamoDbContext;

            _classUnderTest = new DynamoDbGateway(_dynamoDb);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                foreach (var action in _cleanup)
                    action();

                _disposed = true;
            }
        }

        private PersonQueryObject ConstructQuery(Guid id)
        {
            return new PersonQueryObject() { Id = id };
        }

        private Entity ConstructPerson()

        {
            var person = _fixture.Build<Entity>()
                            .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30).ToString())
                            .Create();

            return person;
        }

        private static PersonRequestObject ConstructRequest(Guid id)
        {
            id = Guid.NewGuid();
            return new PersonRequestObject()
            {
                Surname = "Update"
            };
        }

        [Fact]
        public async Task GetPersonByIdReturnsNullIfEntityDoesntExist()
        {
            // Act
            var id = Guid.NewGuid();
            var query = ConstructQuery(id);
            var response = await _classUnderTest.GetEntityById(query).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByIdReturnsThePersonIfItExists()
        {
            // Arrange
            var entity = ConstructPerson();


            var dbEntity = entity.ToDatabase();

            await _dynamoDb.SaveAsync(dbEntity).ConfigureAwait(false);
            _cleanup.Add(async () => await _dynamoDb.DeleteAsync(dbEntity).ConfigureAwait(false));

            // Act
            var query = ConstructQuery(entity.Id);
            var response = await _classUnderTest.GetEntityById(query).ConfigureAwait(false);

            // Assert
            response.DateOfBirth.Should().Be(entity.DateOfBirth);
            response.FirstName.Should().Be(entity.FirstName);
            response.Surname.Should().Be(entity.Surname);
            response.Id.Should().Be(entity.Id);
            response.MiddleName.Should().Be(entity.MiddleName);
            response.PlaceOfBirth.Should().Be(entity.PlaceOfBirth);
            response.Title.Should().Be(entity.Title);

        }

        [Fact]
        public void GetPersonByIdExceptionThrow()
        {
            // Arrange
            var mockDynamoDb = new Mock<IDynamoDBContext>();
            _classUnderTest = new DynamoDbGateway(mockDynamoDb.Object);
            var id = Guid.NewGuid();
            var query = ConstructQuery(id);
            var exception = new ApplicationException("Test exception");
            mockDynamoDb.Setup(x => x.LoadAsync<DatabaseEntity>(id, default))
                        .ThrowsAsync(exception);

            // Act
            Func<Task<Entity>> func = async () => await _classUnderTest.GetEntityById(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
            mockDynamoDb.Verify(x => x.LoadAsync<DatabaseEntity>(id, default), Times.Once);
        }

        [Fact]
        public async Task PostNewTenureSuccessfulSaves()
        {
            // Arrange
            var entityRequest = _fixture.Build<PersonRequestObject>()
                                 .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30).ToString())
                                 .Create();

            // Act
            _ = await _classUnderTest.PostNewPersonAsync(entityRequest).ConfigureAwait(false);

            // Assert
            var dbEntity = await _dynamoDb.LoadAsync<DatabaseEntity>(entityRequest.Id).ConfigureAwait(false);

            dbEntity.Should().BeEquivalentTo(entityRequest.ToDatabase());

            _cleanup.Add(async () => await _dynamoDb.DeleteAsync(dbEntity).ConfigureAwait(false));
        }

        [Fact]
        public async Task UpdatePersonByIdReturnsNullIfEntityDoesntExist()
        {
            // Act
            var id = Guid.NewGuid();
            var query = ConstructQuery(id);
            var constructRequest = ConstructRequest(query.Id);

            var response = await _classUnderTest.UpdatePersonByIdAsync(constructRequest, query).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task UpdatePersonByIdReturnsResponseIfEntityExist()
        {
            // Act

            var entity = ConstructPerson();


            var dbEntity = entity.ToDatabase();

            await _dynamoDb.SaveAsync(dbEntity).ConfigureAwait(false);
            _cleanup.Add(async () => await _dynamoDb.DeleteAsync(dbEntity).ConfigureAwait(false));

            var query = ConstructQuery(entity.Id);
            var constructRequest = ConstructRequest(query.Id);

            var response = await _classUnderTest.UpdatePersonByIdAsync(constructRequest, query).ConfigureAwait(false);

            // Assert
            response.Surname.Should().Be(constructRequest.Surname);
        }

        [Fact]
        public void UpdatePersonByIdExceptionThrow()
        {
            // Arrange
            var mockDynamoDb = new Mock<IDynamoDBContext>();
            _classUnderTest = new DynamoDbGateway(mockDynamoDb.Object);
            var id = Guid.NewGuid();
            var query = ConstructQuery(id);
            var constructRequest = ConstructRequest(query.Id);
            var exception = new ApplicationException("Test exception");
            mockDynamoDb.Setup(x => x.LoadAsync<DatabaseEntity>(id, default))
                        .ThrowsAsync(exception);

            // Act
            Func<Task<Entity>> func = async () => await _classUnderTest.UpdatePersonByIdAsync(constructRequest, query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
            mockDynamoDb.Verify(x => x.LoadAsync<DatabaseEntity>(id, default), Times.Once);
        }


    }
}
