using DynamodbTraining.V1.Boundary.Request;
using DynamodbTraining.V1.Boundary.Request.Validation;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DynamodbTraining.Tests.V1.Boundary.Validation
{
    public class PersonRequestValidationTests
    {
        private readonly PersonRequestObjectValidation _classUnderTest;

        public PersonRequestValidationTests()
        {
            _classUnderTest = new PersonRequestObjectValidation();
        }
        private const string StringWithTags = "Some string with <tag> in it.";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FirstnameShouldNotErrorWithValue(string invalid)
        {
            var model = new PersonRequestObject() { FirstName = invalid };
            var result = _classUnderTest.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void FirstnameShouldErrorWithWithTagsInValue()
        {
            var model = new PersonRequestObject() { FirstName = StringWithTags };
            var result = _classUnderTest.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MiddleNameShouldNotErrorWithValue(string invalid)
        {
            var model = new PersonRequestObject() { MiddleName = invalid };
            var result = _classUnderTest.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
        }

        [Fact]
        public void MiddleNameShouldErrorWithWithTagsInValue()
        {
            var model = new PersonRequestObject() { MiddleName = StringWithTags };
            var result = _classUnderTest.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MiddleName)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SurnameShouldNotErrorWithValue(string invalid)
        {
            var model = new PersonRequestObject() { Surname = invalid };
            var result = _classUnderTest.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Surname);
        }

        [Fact]
        public void SurnameShouldErrorWithWithTagsInValue()
        {
            var model = new PersonRequestObject() { Surname = StringWithTags };
            var result = _classUnderTest.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Surname)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PlaceOfBirthShouldNotErrorWithValue(string invalid)
        {
            var model = new PersonRequestObject() { PlaceOfBirth = invalid };
            var result = _classUnderTest.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PlaceOfBirth);
        }

        [Fact]
        public void PlaceOfBirthShouldErrorWithWithTagsInValue()
        {
            var model = new PersonRequestObject() { PlaceOfBirth = StringWithTags };
            var result = _classUnderTest.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PlaceOfBirth)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
        }
    }
}
