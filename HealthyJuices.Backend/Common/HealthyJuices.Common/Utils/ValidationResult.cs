using System.Collections.Generic;

namespace HealthyJuices.Common.Utils
{
    public record ValidationResult
    {
        public bool Faild { get; set; } = false;

        public IEnumerable<string> Errors { get; init; } = new List<string>();

        public static ValidationResult Success => new ValidationResult();
        public static ValidationResult Fail(IEnumerable<string> errors) => new ValidationResult { Faild = true, Errors = errors };
    }
}