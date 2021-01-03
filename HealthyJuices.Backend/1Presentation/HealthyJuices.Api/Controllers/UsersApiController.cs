using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Controllers;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersApiController : ApiController
    {
        private readonly UsersController _appController;

        public UsersApiController(UsersController appController)
        {
            _appController = appController;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<UserDto>> GetAllAsync()
        {
            var result = await _appController.GetAllAsync();
            return result;
        }

        [Authorize]
        [HttpGet("active")]
        public async Task<List<UserDto>> GetAllActiveAsync()
        {
            var result = await _appController.GetAllActiveAsync();
            return result;
        }

        [Authorize]
        [HttpGet("role/{role}")]
        public async Task<List<UserDto>> GetAllActiveByUserRoleAsync(UserRole role)
        {
            var result = await _appController.GetAllActiveByUserRoleAsync(role);
            return result;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<UserDto> GetByIdAsync(long id)
        {
            var result = await _appController.GetAsync(id);
            return result;
        }

        [Authorize]
        [HttpGet("{userName}")]
        public async Task<UserDto> GetAsync(string userName)
        {
            var result = await _appController.GetAsync(userName);
            return result;
        }

        [HttpGet("existing/{userName}")]
        [AllowAnonymous]
        public async Task<bool> IsExistingAsync(string userName)
        {
            var result = await _appController.IsExistingAsync(userName);
            return result;
        }

        [Authorize]
        [HttpPost]
        public async Task<long> CreateAsync([FromBody] AddOrEditUserDto dto)
        {
            var result = await _appController.CreateAsync(dto);
            return result;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task DeleteAsync(long id)
        {
            await _appController.DeleteAsync(id);
        }
    }
}