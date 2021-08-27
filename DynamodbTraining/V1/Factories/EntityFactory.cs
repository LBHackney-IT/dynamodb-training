using DynamodbTraining.V1.Domain;
using DynamodbTraining.V1.Infrastructure;

namespace DynamodbTraining.V1.Factories
{
    public static class EntityFactory
    {
        public static Entity ToDomain(this DatabaseEntity databaseEntity)
        {

            return new Entity
            {
                Id = databaseEntity.Id,
                DateOfBirth = databaseEntity.DateOfBirth,
                FirstName = databaseEntity.FirstName,
                MiddleName = databaseEntity.MiddleName,
                PlaceOfBirth = databaseEntity.PlaceOfBirth,
                Surname = databaseEntity.Surname,
                Title = databaseEntity.Title
            };
        }

        public static DatabaseEntity ToDatabase(this Entity entity)
        {

            return new DatabaseEntity
            {
                Id = entity.Id,
                Title = entity.Title,
                Surname = entity.Surname,
                PlaceOfBirth = entity.PlaceOfBirth,
                MiddleName =entity.MiddleName,
                FirstName = entity.FirstName,
                DateOfBirth = entity.DateOfBirth
            };
        }
    }
}
