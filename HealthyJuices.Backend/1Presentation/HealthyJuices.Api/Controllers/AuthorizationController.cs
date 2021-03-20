using System.Threading.Tasks;
using HealthyJuices.Application.Services;
using HealthyJuices.Application.Services.Auth.Commands;
using HealthyJuices.Application.Services.Auth.Queries;
using HealthyJuices.Application.Services.Companies.Queries;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Dto.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthorizationController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthorizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] Login.Query query)
        {
            var response = await _mediator.Send(query);
            return ToActionResult(response);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] Register.Command command)
        {
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [AllowAnonymous]
        [HttpGet("confirm-register")]
        public async Task<IActionResult> ConfirmRegisterAsync([FromQuery] string email, [FromQuery] string token)
        {
            var response = await _mediator.Send(new ConfirmRegister.Command(email, token));
            return ToActionResult(response);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPassword.Command command)
        {
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPassword.Command command)
        {
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }
    }
}