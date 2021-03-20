using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using HealthyJuices.Common.Utils;
using HealthyJuices.Shared.Enums;
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

        public IActionResult ToActionResult<T>(Response<T> response)
        {
            if (response == null)
                return BadRequest();

            if (response.Succeed)
                return Ok(response.Value);

            return ToActionResult(response as Response);
        }


        public IActionResult ToActionResult(Response response)
        {
            if (response == null)
                return BadRequest();

            if (response.Succeed)
                return Ok();

            switch (response.Status)
            {
                case ResponseStatus.Success:
                    return Ok();

                case ResponseStatus.NotFound:
                    return NotFound(response.Message);

                case ResponseStatus.BadQuery:
                    return BadRequest(response.Message);

                case ResponseStatus.ValidationError:
                    return Forbid(response.Message);

                case ResponseStatus.DataBaseError:
                    return BadRequest(response.Message);

                case ResponseStatus.BussinesLogicError:
                    return BadRequest(response.Message);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}