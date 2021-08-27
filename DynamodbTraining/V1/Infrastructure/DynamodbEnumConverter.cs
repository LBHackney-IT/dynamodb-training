using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.Infrastructure
{
    /// <summary>
    /// Converter for enums where the value stored should be the enum value name (not the numeric value)
    /// </summary>
    public class DynamoDbEnumConverter<TEnum> : IPropertyConverter where TEnum : Enum
    {
        public DynamoDBEntry ToEntry(object value)
        {
            if (null == value) return new DynamoDBNull();

            return new Primitive(Enum.GetName(typeof(TEnum), value));
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            Primitive primitive = entry as Primitive;
            var entryStringValue = primitive?.AsString();
            if (string.IsNullOrEmpty(entryStringValue)) return default(TEnum);

            TEnum valueAsEnum = (TEnum) Enum.Parse(typeof(TEnum), entryStringValue);
            return valueAsEnum;
        }
    }
}
