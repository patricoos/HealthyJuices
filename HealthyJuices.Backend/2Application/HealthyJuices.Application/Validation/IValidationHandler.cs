using HealthyJuices.Common.Utils;
using System.Threading.Tasks;

namespace HealthyJuices.Application.Validation
{
    public interface IValidationHandler { }

    public interface IValidationHandler<T> : IValidationHandler
    {
        Task<ValidationResult> Validate(T request);
    }
}
