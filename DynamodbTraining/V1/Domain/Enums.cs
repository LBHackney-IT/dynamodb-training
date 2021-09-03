using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.Domain
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Title
    {
        Dr,
        Master,
        Miss,
        Mr,
        Mrs,
        Ms
    }

}
