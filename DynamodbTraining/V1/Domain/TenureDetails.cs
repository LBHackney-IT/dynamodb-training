using System;
using System.Text;
using System.Text.Json.Serialization;

namespace DynamodbTraining.V1.Domain
{
    public class TenureDetails
    {
        public string AssetFullAddress { get; set; }

        public string AssetId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Uprn { get; set; }

        public string PaymentReference { get; set; }

        public string PropertyReference { get; set; }

        public override int GetHashCode()
        {
            StringBuilder builder = new StringBuilder();
            return builder.Append(AssetFullAddress)
                          .Append(AssetId)
                          .Append(StartDate)
                          .Append(EndDate)
                          .Append(Id.ToString())
                          .Append(Type)
                          .Append(Uprn)
                          .Append(PaymentReference)
                          .Append(PropertyReference)
                          .ToString()
                          .GetHashCode();
        }
    }
}
