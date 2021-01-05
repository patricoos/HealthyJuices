using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected long RequestSenderId
        {
            get
            {
                var customerClaim = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name);
                if (customerClaim != null && int.TryParse(customerClaim.Value, out var id))
                    return id;

                throw new UnhandledException("User id Not In Token");
            }
        }

        protected string IpAddress
        {
            get
            {
                if (Request.Headers.ContainsKey("X-Forwarded-For"))
                    return Request.Headers["X-Forwarded-For"];
                else
                    return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}