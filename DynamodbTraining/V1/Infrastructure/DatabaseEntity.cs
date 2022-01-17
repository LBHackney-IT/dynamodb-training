using Amazon.DynamoDBv2.DataModel;
using DynamodbTraining.V1.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamodbTraining.V1.Infrastructure
{

    [DynamoDBTable("dynamodb-training", LowerCamelCaseProperties = true)]
    public class DatabaseEntity
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbEnumConverter<Title>))]
        public Title? Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string PlaceOfBirth { get; set; }

        //[DynamoDBProperty(Converter = typeof(DynamoDbDateTimeConverter))]
        public string DateOfBirth { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbObjectListConverter<TenureDetails>))]
        public List<TenureDetails> Tenures { get; set; } = new List<TenureDetails>();

    }
}
