using System.Linq;
using System.Security.Claims;
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
        protected string RequestSenderId => User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}