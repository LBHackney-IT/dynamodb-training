using AutoFixture;
using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Factories;
using DynamodbTraining.V1.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class PostE2ETests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        public DatabaseEntity Person { get; private set; }
        private readonly DynamoDbIntegrationTests<Startup> _dbFixture;
        private readonly List<Action> _cleanupActions = new List<Action>();
        private ResponseFactory _responseFactory;


        public PostE2ETests(DynamoDbIntegrationTests<Startup> dbFixture)
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

        private PersonRequestObject GivenANewPersonRequest()
        {
            var personRequest = _fixture.Build<PersonRequestObject>()
                .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30).ToString())
                .Create();

            return personRequest;

        }

        private PersonRequestObject GivenANewPersonRequestWithValidationErrors()
        {
            var personRequest = _fixture.Build<PersonRequestObject>()
               .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30).ToString())
               .With(x => x.FirstName, "Some string with <tag> in it.")
               .With(x => x.Surname, "Some string with <tag> in it.")
               .With(x => x.MiddleName, "Some string with <tag> in it.")
               .With(x => x.PlaceOfBirth, "Some string with <tag> in it.")
               .Create();

            return personRequest;
        }

        [Fact]
        public async Task PostPersonReturnsCreated()
        {
            var requestObject = GivenANewPersonRequest();

            var uri = new Uri($"api/v1/residents", UriKind.Relative);
            var content = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json");
            var response = await _dbFixture.Client.PostAsync(uri, content).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiPerson = JsonSerializer.Deserialize<PersonResponseObject>(responseContent, CreateJsonOptions());

            apiPerson.Id.Should().NotBeEmpty();

            var dbRecord = await _dbFixture.DynamoDbContext.LoadAsync<DatabaseEntity>(apiPerson.Id).ConfigureAwait(false);
            var domain = dbRecord.ToDomain();
            apiPerson.Should().BeEquivalentTo(_responseFactory.ToResponse(domain));
        }

        protected JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        [Fact]
        public async Task PostPersonReturnsBadRequestWithValidationErrors()
        {
            var requestObject = GivenANewPersonRequestWithValidationErrors();

            var uri = new Uri($"api/v1/residents", UriKind.Relative);
            var content = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json");
            var response = await _dbFixture.Client.PostAsync(uri, content).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            JObject jo = JObject.Parse(responseContent);
            var errors = jo["errors"].Children();

            ShouldHaveErrorFor(errors, "FirstName");
            ShouldHaveErrorFor(errors, "Surname");
            ShouldHaveErrorFor(errors, "MiddleName");
            ShouldHaveErrorFor(errors, "PlaceOfBirth");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }

        private static void ShouldHaveErrorFor(JEnumerable<JToken> errors, string propertyName, string errorCode = null)
        {
            var error = errors.FirstOrDefault(x => (x.Path.Split('.').Last().Trim('\'', ']')) == propertyName) as JProperty;
            error.Should().NotBeNull();
            if (!string.IsNullOrEmpty(errorCode))
                error.Value.ToString().Should().Contain(errorCode);
        }

    }
}
