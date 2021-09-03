using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.Factories
{
    public static class CreateRequestFactory
    {
        public static DatabaseEntity ToDatabase(this PersonRequestObject createRequestObject)
        {
            return new DatabaseEntity()
            {
                Id = createRequestObject.Id == Guid.Empty ? Guid.NewGuid() : createRequestObject.Id,
                DateOfBirth = createRequestObject.DateOfBirth,
                FirstName = createRequestObject.FirstName,
                MiddleName = createRequestObject.MiddleName,
                PlaceOfBirth = createRequestObject.PlaceOfBirth,
                Surname = createRequestObject.Surname,
                Title = createRequestObject.Title

            };

        }
    }
}
