using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HealthyJuices.Application.Services;
using HealthyJuices.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        #region Fields & Constructors
        protected string RequestSenderId => User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        protected string IpAddress
        {
            get
            {
                if (Request.Headers.ContainsKey("X-Forwarded-For"))
                    return Request.Headers["X-Forwarded-For"];

                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }

        #endregion Fields & Constructors

        #region Methods

        protected async Task<UserDto> GetCurrentUser(UsersService userService)
        {
            var userId = RequestSenderId;
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            var userDto = await userService.GetAsync(userId);
            return userDto;
        }

        #endregion Methods
    }
}