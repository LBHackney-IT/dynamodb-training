using System;

namespace DynamodbTraining.V1.Domain
{
    public class Entity
    {
        public Guid Id { get; set; }
        public Title? Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string PlaceOfBirth { get; set; }
        public string DateOfBirth { get; set; }
    }
}
