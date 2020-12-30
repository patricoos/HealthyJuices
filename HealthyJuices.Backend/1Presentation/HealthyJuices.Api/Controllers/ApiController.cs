using System.Threading.Tasks;
using HealthyJuices.Api.Auth;
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
                if (!HttpContext.Items.TryGetValue("UserId", out var userId)) return -1;

                if (long.TryParse(userId.ToString(), out var result))
                    return result;

                return -1;
            }
        }
    }
}