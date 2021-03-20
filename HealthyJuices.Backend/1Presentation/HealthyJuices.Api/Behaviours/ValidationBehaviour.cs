using System.Net;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Validation;
using HealthyJuices.Common.Utils;
using HealthyJuices.Shared.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthyJuices.Api.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : Response, new()
    {
        private readonly IValidationHandler<TRequest> _validationHandler;

        // Have 2 constructors incase the validator does not exist
        public ValidationBehaviour()
        {
        }

        public ValidationBehaviour(IValidationHandler<TRequest> validationHandler)
        {
            this._validationHandler = validationHandler;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType();
            if (_validationHandler == null)
                return await next();

            var result = await _validationHandler.Validate(request);
            if (result.Faild)
                return new TResponse { Status = ResponseStatus.ValidationError, Errors = result.Errors };

            return await next();
        }
    }
}
