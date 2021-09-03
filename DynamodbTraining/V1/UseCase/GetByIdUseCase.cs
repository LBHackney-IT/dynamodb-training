using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Domain;
using DynamodbTraining.V1.Factories;
using DynamodbTraining.V1.Gateways;
using DynamodbTraining.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.UseCase
{
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private IExampleGateway _gateway;
        public GetByIdUseCase(IExampleGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<Entity> Execute(PersonQueryObject personQueryObject)
        {
            return await _gateway.GetEntityById(personQueryObject).ConfigureAwait(false);
        }
    }
}
