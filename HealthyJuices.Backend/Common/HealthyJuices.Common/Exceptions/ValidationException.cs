﻿using System.Collections.Generic;
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

        public ValidationException(IEnumerable<ValidationFailure> failures) : base(HttpStatusCode.Forbidden, ValidationFailuresToMessage(failures))
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        private static string ValidationFailuresToMessage(IEnumerable<ValidationFailure> failures)
        {
            failures.Select(x => $"{x.ErrorMessage}");
            return string.Join(",\n", failures);
        }
    }
}