using System.Threading.Tasks;
using HealthyJuices.Application.Services;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthorizationApiController : ApiController
    {
        private readonly AuthorizationService _service;

        public AuthorizationApiController(AuthorizationService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<LoginResponseDto> LoginAsync([FromBody] LoginDto dto)
        {
            var result = await _service.LoginAsync(dto);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("login-refresh-token")]
        public async Task<LoginResponseDto> LoginWithRefreshTokenAsync([FromBody] LoginDto dto)
        {
            var result = await _service.LoginWithRefreshTokenAsync(dto, IpAddress);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<LoginResponseDto> RefreshTokenAsync([FromBody] TokenDto model)
        {
            var result = await _service.RefreshTokenAsync(model.Token, IpAddress);
            return result;
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] TokenDto model)
        {
            await _service.RevokeTokenAsync(model.Token, IpAddress);
            return Ok(new { message = "Token revoked" });
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task RegisterAsync([FromBody] RegisterUserDto dto)
        {
            await _service.RegisterAsync(dto);
        }

        [AllowAnonymous]
        [HttpGet("confirm-register")]
        public async Task ConfirmRegisterAsync([FromQuery] string email, [FromQuery] string token)
        {
            await _service.ConfirmRegisterAsync(email, token);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task ForgotPasswordAsync([FromBody] ForgotPasswordDto dto)
        {
            await _service.ForgotPasswordAsync(dto);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task ResetPasswordAsync([FromBody] ResetPasswordDto dto)
        {
            await _service.ResetPasswordAsync(dto);
        }
    }
}