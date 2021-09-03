using AutoFixture;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Domain;
using DynamodbTraining.V1.Factories;
using DynamodbTraining.V1.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DynamodbTraining.Tests.V1.E2ETests
{
    [Collection("DynamoDb collection")]
    public class GetByIdE2ETests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        public DatabaseEntity Person { get; private set; }
        private readonly DynamoDbIntegrationTests<Startup> _dbFixture;
        private readonly List<Action> _cleanupActions = new List<Action>();
        private ResponseFactory _responseFactory;

        public GetByIdE2ETests(DynamoDbIntegrationTests<Startup> dbFixture)
        {
            _dbFixture = dbFixture;
            _responseFactory = new ResponseFactory();
        }

        /// <summary>
        /// Method to construct a test entity that can be used in a test
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Entity ConstructTestEntity()
        {
            var entity = _fixture.Build<Entity>()
                             .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30).ToString())
                            .Create();

            return entity;
        }
        /// <summary>
        /// Method to add an entity instance to the database so that it can be used in a test.
        /// Also adds the corresponding action to remove the upserted data from the database when the test is done.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task SetupTestData(Entity entity)
        {
            await _dbFixture.DynamoDbContext.SaveAsync(entity.ToDatabase()).ConfigureAwait(false);
            _cleanupActions.Add(async () => await _dbFixture.DynamoDbContext.DeleteAsync<DatabaseEntity>(entity.Id).ConfigureAwait(false));
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
                foreach (var action in _cleanupActions)
                    action();

                _disposed = true;
            }
        }

        [Fact]
        public async Task GetEntityByIdNotFoundReturns404()
        {
            var id = Guid.NewGuid();
            var uri = new Uri($"api/v1/residents/{id}", UriKind.Relative);
            var response = await _dbFixture.Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task GetPersonByIdFoundReturnsResponse()
        {
            var entity = ConstructTestEntity();
            await SetupTestData(entity).ConfigureAwait(false);

            var uri = new Uri($"api/v1/residents/{entity.Id}", UriKind.Relative);
            var response = await _dbFixture.Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonConvert.DeserializeObject<PersonResponseObject>(responseContent);

            apiEntity.Should().BeEquivalentTo(_responseFactory.ToResponse(entity));
        }
    }
}
