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
                Id = domain.Id,
                Tenures = SortTenures(domain.Tenures)
            };
        }

        private static List<TenureResponseObject> SortTenures(IEnumerable<TenureDetails> tenures)
        {
            if (tenures == null) return null;

            var sortedTenures = tenures
                .OrderByDescending(x => x.Type == "Secure")
                .ThenByDescending(ParseTenureStartDate)
                .ToList();

            return sortedTenures.Select(x => ToResponse(x)).ToList();
        }

        private static DateTime? ParseTenureStartDate(TenureDetails tenure)
        {
            DateTime parsedDate;
            if (DateTime.TryParse(tenure.StartDate, out parsedDate)) return (DateTime?) parsedDate;

            return null;
        }

        public static TenureResponseObject ToResponse(TenureDetails tenure)
        {
            return new TenureResponseObject()
            {
                AssetFullAddress = tenure.AssetFullAddress,
                AssetId = tenure.AssetId,
                EndDate = tenure.EndDate,
                Id = tenure.Id,
                PaymentReference = tenure.PaymentReference,
                PropertyReference = tenure.PropertyReference,
                StartDate = tenure.StartDate,
                Type = tenure.Type,
                Uprn = tenure.Uprn
            };
        }
    }
}
