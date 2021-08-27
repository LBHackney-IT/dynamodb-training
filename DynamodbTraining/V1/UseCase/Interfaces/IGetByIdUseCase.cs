using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Domain;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        Task<Entity> Execute(PersonQueryObject personQueryObject);
    }
}
