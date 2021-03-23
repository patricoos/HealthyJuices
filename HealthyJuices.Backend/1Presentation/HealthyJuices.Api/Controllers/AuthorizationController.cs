using System.Threading.Tasks;
using HealthyJuices.Application.Functions.Auth.Commands;
using HealthyJuices.Application.Functions.Auth.Commands;
using HealthyJuices.Application.Functions.Auth.Queries;
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
        public async Task<LoginResponseDto> LoginAsync([FromBody] Login.Query query)
        {
            var response = await _mediator.Send(query);
            return response;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<string> RegisterAsync([FromBody] Register.Command command)
        {
            var response = await _mediator.Send(command);
            return response;
        }

        [AllowAnonymous]
        [HttpGet("confirm-register")]
        public async Task ConfirmRegisterAsync([FromQuery] string email, [FromQuery] string token)
        {
            await _mediator.Send(new ConfirmRegister.Command(email, token));
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task ForgotPasswordAsync([FromBody] ForgotPassword.Command command)
        {
            var response = await _mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task ResetPasswordAsync([FromBody] ResetPassword.Command command)
        {
            await _mediator.Send(command);
        }
    }
}