using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Services;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
        [Authorize]
    [Route("users")]
    public class UsersController : BaseApiController
    {
        private readonly UsersService _service;

        public UsersController(UsersService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<UserDto>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return result;
        }

        [HttpGet("active")]
        public async Task<List<UserDto>> GetAllActiveAsync()
        {
            var result = await _service.GetAllActiveAsync();
            return result;
        }

        [HttpGet("role/{role}")]
        public async Task<List<UserDto>> GetAllActiveByUserRoleAsync(UserRole role)
        {
            var result = await _service.GetAllActiveByUserRoleAsync(role);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<UserDto> GetByIdAsync(string id)
        {
            var result = await _service.GetAsync(id);
            return result;
        }

        [HttpGet("{userName}")]
        public async Task<UserDto> GetAsync(string userName)
        {
            var result = await _service.GetAsync(userName);
            return result;
        }

        [AllowAnonymous]
        [HttpGet("existing/{userName}")]
        public async Task<bool> IsExistingAsync(string userName)
        {
            var result = await _service.IsExistingAsync(userName);
            return result;
        }

        [HttpPost]
        public async Task<string> CreateAsync([FromBody] AddOrEditUserDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _service.DeleteAsync(id);
        }
    }
}