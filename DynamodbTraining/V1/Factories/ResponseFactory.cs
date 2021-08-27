using System;
using System.Collections.Generic;
using System.Linq;
using DynamodbTraining.V1.Boundary.Response;
using DynamodbTraining.V1.Domain;

namespace DynamodbTraining.V1.Factories
{
    public class ResponseFactory : IResponseFactory
    {
        public static string FormatDateOfBirth(DateTime? dob)
        {
            return dob?.ToString("yyyy-MM-dd");
        }

        public PersonResponseObject ToResponse(Entity domain)
        {
            if (domain == null) return null;

            return new PersonResponseObject()
            {
                DateOfBirth = domain.DateOfBirth,
                FirstName = domain.FirstName,
                MiddleName = domain.MiddleName,
                PlaceOfBirth = domain.PlaceOfBirth,
                Surname = domain.Surname,
                Title = domain.Title,
                Id = domain.Id
            };
        }
    }
}
