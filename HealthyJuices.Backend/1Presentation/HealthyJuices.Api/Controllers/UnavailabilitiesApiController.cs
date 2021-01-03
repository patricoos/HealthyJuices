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
    [Route("unavailabilities")]
    [Authorize]
    public class UnavailabilitiesApiController : ApiController
    {
        private readonly UnavailabilitiesController _appController;

        public UnavailabilitiesApiController(UnavailabilitiesController appController)
        {
            _appController = appController;
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<List<UnavailabilityDto>> GetAllAsync()
        {
            var result = await _appController.GetAllAsync();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<UnavailabilityDto> GetByIdAsync(long id)
        {
            var result = await _appController.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<long> CreateAsync(UnavailabilityDto dto)
        {
            var result = await _appController.CreateAsync(dto);
            return result;
        }

        [HttpPut]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(UnavailabilityDto dto)
        {
            await _appController.UpdateAsync(dto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task DeleteAsync(long id)
        {
            await _appController.DeleteByIdAsync(id);
        }
    }
}