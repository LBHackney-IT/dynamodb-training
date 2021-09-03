using Amazon.DynamoDBv2.DataModel;
using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Domain;
using DynamodbTraining.V1.Factories;
using DynamodbTraining.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.Gateways
{
    public class DynamoDbGateway : IExampleGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<Entity> GetEntityById(PersonQueryObject query)
        {
            var result = await _dynamoDbContext.LoadAsync<DatabaseEntity>(query.Id).ConfigureAwait(false);
            return result?.ToDomain();
        }
        public async Task<Entity> PostNewPersonAsync(PersonRequestObject requestObject)
        {
            var personDbEntity = requestObject.ToDatabase();

            await _dynamoDbContext.SaveAsync(personDbEntity).ConfigureAwait(false);

            return personDbEntity.ToDomain();
        }

        public async Task<Entity> UpdatePersonByIdAsync(PersonRequestObject requestObject, PersonQueryObject query)
        {

            var personDbEntity = requestObject.ToDatabase();
            personDbEntity.Id = query.Id;
            var load = await _dynamoDbContext.LoadAsync<DatabaseEntity>(query.Id).ConfigureAwait(false);

            if (load == null) return null;


            await _dynamoDbContext.SaveAsync(personDbEntity, new DynamoDBOperationConfig { IgnoreNullValues = true }).ConfigureAwait(false);

            return personDbEntity?.ToDomain();
        }
    }
}
