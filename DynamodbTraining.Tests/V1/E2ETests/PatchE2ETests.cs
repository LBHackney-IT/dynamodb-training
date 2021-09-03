using AutoFixture;
using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Domain;
using DynamodbTraining.V1.Factories;
using DynamodbTraining.V1.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DynamodbTraining.Tests.V1.E2ETests
{
    [Collection("DynamoDb collection")]
    public class PatchE2ETests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        public DatabaseEntity Person { get; private set; }
        private readonly DynamoDbIntegrationTests<Startup> _dbFixture;
        private readonly List<Action> _cleanupActions = new List<Action>();
        private ResponseFactory _responseFactory;


        public PatchE2ETests(DynamoDbIntegrationTests<Startup> dbFixture)
        {
            _dbFixture = dbFixture;
            _responseFactory = new ResponseFactory();
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

        public DatabaseEntity GivenAPersonAlreadyExistsAndUpdateRequested()
        {
            if (null == Person)
            {
                var person = _fixture.Build<DatabaseEntity>()
                                     .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30).ToString())
                                    .Create();
                _dbFixture.DynamoDbContext.SaveAsync<DatabaseEntity>(person).GetAwaiter().GetResult();
                Person = person;
            }
            return Person;
        }

        public PersonRequestObject GivenAUpdatePersonRequest(Guid id)
        {
            var personRequest = new PersonRequestObject()
            {
                Id = id,
                FirstName = "Update",
                Surname = "Updating",
                Title = Title.Dr
            };

            return personRequest;
        }

        [Fact]
        public async Task UpdatedPersonReturnsNoContent()
        {
            var person = GivenAPersonAlreadyExistsAndUpdateRequested();
            var updateRequest = GivenAUpdatePersonRequest(person.Id);


            var uri = new Uri($"api/v1/residents/{person.Id}", UriKind.Relative);
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            var response = await _dbFixture.Client.PatchAsync(uri, content).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task UpdatePersonReturnsNotFound()
        {
            var id = Guid.NewGuid();
            var updateRequest = GivenAUpdatePersonRequest(id);

            var uri = new Uri($"api/v1/residents/{id}", UriKind.Relative);
            var content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json");
            var response = await _dbFixture.Client.PatchAsync(uri, content).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }
    }
}
