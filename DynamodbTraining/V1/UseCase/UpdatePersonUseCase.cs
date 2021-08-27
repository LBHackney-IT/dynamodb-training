using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Factories;
using DynamodbTraining.V1.Gateways;
using DynamodbTraining.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.UseCase
{
    public class UpdatePersonUseCase : IUpdatePersonUseCase
    {
        private readonly IExampleGateway _gateway;
        private readonly IResponseFactory _responseFactory;

        public UpdatePersonUseCase(IExampleGateway gateway, IResponseFactory responseFactory)
        {
            _gateway = gateway;
            _responseFactory = responseFactory;
        }

        public async Task<PersonResponseObject> ExecuteAsync(PersonRequestObject personRequestObject, PersonQueryObject query)
        {
            var person = await _gateway.UpdatePersonByIdAsync(personRequestObject, query).ConfigureAwait(false);
            return _responseFactory.ToResponse(person);
        }
    }
}
