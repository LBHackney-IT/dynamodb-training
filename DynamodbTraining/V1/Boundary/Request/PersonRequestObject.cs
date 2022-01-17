using DynamodbTraining.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.Boundary.Request
{
    public class PersonRequestObject
    {
        public Guid Id { get; set; }

        public Title? Title { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string PlaceOfBirth { get; set; }

        public string DateOfBirth { get; set; }
        public string PreferredTitle { get; set; }
        public string PreferredFirstName { get; set; }
        public string PreferredMiddleName { get; set; }
        public string PreferredSurname { get; set; }
    }
}
