using System.Linq;
using System.Security.Claims;
using HealthyJuices.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorDetailsDto), 400)]
    [ProducesResponseType(typeof(ErrorDetailsDto), 401)]
    [ProducesResponseType(typeof(ErrorDetailsDto), 409)]
    [ProducesResponseType(typeof(ErrorDetailsDto), 500)]

    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected string RequestSenderId => User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}