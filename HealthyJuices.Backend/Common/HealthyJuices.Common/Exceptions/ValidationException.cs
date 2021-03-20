using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation.Results;

namespace HealthyJuices.Common.Exceptions
{
    public class ValidationException : CustomException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(string message, params string[] translationParams)
            : base(HttpStatusCode.Forbidden, message, translationParams)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : base(HttpStatusCode.Forbidden, "Validation Error")
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}