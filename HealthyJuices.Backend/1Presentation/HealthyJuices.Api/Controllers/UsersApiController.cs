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
    [Route("users")]
    public class UsersApiController : ApiController
    {
        private readonly UsersService _service;

        public UsersApiController(UsersService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<UserDto>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return result;
        }

        [Authorize]
        [HttpGet("active")]
        public async Task<List<UserDto>> GetAllActiveAsync()
        {
            var result = await _service.GetAllActiveAsync();
            return result;
        }

        [Authorize]
        [HttpGet("role/{role}")]
        public async Task<List<UserDto>> GetAllActiveByUserRoleAsync(UserRole role)
        {
            var result = await _service.GetAllActiveByUserRoleAsync(role);
            return result;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<UserDto> GetByIdAsync(long id)
        {
            var result = await _service.GetAsync(id);
            return result;
        }

        [Authorize]
        [HttpGet("{userName}")]
        public async Task<UserDto> GetAsync(string userName)
        {
            var result = await _service.GetAsync(userName);
            return result;
        }

        [HttpGet("existing/{userName}")]
        [AllowAnonymous]
        public async Task<bool> IsExistingAsync(string userName)
        {
            var result = await _service.IsExistingAsync(userName);
            return result;
        }

        [Authorize]
        [HttpPost]
        public async Task<long> CreateAsync([FromBody] AddOrEditUserDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return result;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task DeleteAsync(long id)
        {
            await _service.DeleteAsync(id);
        }
    }
}