using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Domain;

namespace DynamodbTraining.V1.Gateways
{
    public interface IExampleGateway
    {
        Task<Entity> GetEntityById(PersonQueryObject query);
        Task<Entity> PostNewPersonAsync(PersonRequestObject requestObject);

        Task<Entity> UpdatePersonByIdAsync(PersonRequestObject requestObject, PersonQueryObject query);
    }
}
