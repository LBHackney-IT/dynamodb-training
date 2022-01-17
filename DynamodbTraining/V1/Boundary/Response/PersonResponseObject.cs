using DynamodbTraining.V1.Domain;
using System;
using System.Collections.Generic;

namespace DynamodbTraining.V1.Boundary.Response
{
    public class PersonResponseObject
    {
        public Guid Id { get; set; }

        /// <example>Mr, Mrs, Miss</example>
        public Title? Title { get; set; }

        /// <example>Julie</example>
        public string FirstName { get; set; }
        /// <example></example>
        public string MiddleName { get; set; }
        /// <example>Evans</example>
        public string Surname { get; set; }
        /// <example>London</example>
        public string PlaceOfBirth { get; set; }
        /// <example>1990-02-19</example>
        public string DateOfBirth { get; set; }
        public IEnumerable<TenureResponseObject> Tenures { get; set; }

    }
}
