using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HealthyJuices.Api.Behaviours
{
    public class UserIdPipe<TIn, TOut> : IPipelineBehavior<TIn, TOut>
    {
        private readonly HttpContext _httpContext;

        public UserIdPipe(IHttpContextAccessor accessor)
        {
            _httpContext = accessor.HttpContext;
        }

        public async Task<TOut> Handle(
            TIn request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TOut> next)
        {
            if (request is ISenderRequest br)
            {
                var requestSenderId = _httpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                br.RequestSenderId = requestSenderId;
            }

            return await next(); ;
        }
    }
}