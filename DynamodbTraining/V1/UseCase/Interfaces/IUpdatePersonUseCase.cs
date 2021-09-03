using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.UseCase.Interfaces
{
    public interface IUpdatePersonUseCase
    {
        Task<PersonResponseObject> ExecuteAsync(PersonRequestObject personRequestObject, PersonQueryObject query);
    }
}
