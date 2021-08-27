using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.Factories
{
    public interface IResponseFactory
    {
        PersonResponseObject ToResponse(Entity domain);
    }
}
