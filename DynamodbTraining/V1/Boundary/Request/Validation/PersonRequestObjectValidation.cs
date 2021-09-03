using FluentValidation;
using Hackney.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamodbTraining.V1.Boundary.Request.Validation
{
    public class PersonRequestObjectValidation : AbstractValidator<PersonRequestObject>
    {
        public PersonRequestObjectValidation()
        {
            RuleFor(x => x.FirstName).NotXssString()
                                     .WithErrorCode(ErrorCodes.XssCheckFailure);

            RuleFor(x => x.Surname).NotXssString()
                                     .WithErrorCode(ErrorCodes.XssCheckFailure);
            RuleFor(x => x.PlaceOfBirth).NotXssString()
                                     .WithErrorCode(ErrorCodes.XssCheckFailure);
            RuleFor(x => x.MiddleName).NotXssString()
                                     .WithErrorCode(ErrorCodes.XssCheckFailure);
        }


    }
}
