using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.Boundary.Request
{
    public class PersonQueryObject
    {
        [FromRoute(Name ="id")]
        public Guid Id { get; set; }
    }
}
