using System;
using System.Threading.Tasks;
using HealthyJuices.Application.Controllers;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    public class AuthorizationApiController : ApiController
    {
        private readonly AuthorizationController _appController;

        public AuthorizationApiController(AuthorizationController appController)
        {
            _appController = appController;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<LoginResponseDto> LoginAsync([FromBody] LoginDto dto)
        {
            var result = await _appController.LoginAsync(dto);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("login-refresh-token")]
        public async Task<LoginResponseDto> LoginWithRefreshTokenAsync([FromBody] LoginDto dto)
        {
            var result = await _appController.LoginWithRefreshTokenAsync(dto, IpAddress);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<LoginResponseDto> RefreshTokenAsync([FromBody] TokenDto model)
        {
            var result = await _appController.RefreshTokenAsync(model.Token, IpAddress);
            return result;
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] TokenDto model)
        {
            await _appController.RevokeTokenAsync(model.Token, IpAddress);
            return Ok(new { message = "Token revoked" });
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task RegisterAsync([FromBody] RegisterUserDto dto)
        {
            await _appController.RegisterAsync(dto);
        }

        [AllowAnonymous]
        [HttpPost("confirm-register")]
        public async Task ConfirmRegisterAsync([FromQuery] string email, [FromQuery] string token)
        {
            await _appController.ConfirmRegisterAsync(email, token);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task ForgotPasswordAsync([FromBody] ForgotPasswordDto dto)
        {
            await _appController.ForgotPasswordAsync(dto);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task ResetPasswordAsync([FromBody] ResetPasswordDto dto)
        {
            await _appController.ResetPasswordAsync(dto);
        }
    }
}