using System;
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
    [Route("unavailabilities")]
    [Authorize]
    public class UnavailabilitiesApiController : ApiController
    {
        private readonly UnavailabilitiesService _service;

        public UnavailabilitiesApiController(UnavailabilitiesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<UnavailabilityDto>> GetAllAsync([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var result = await _service.GetAllAsync(from, to);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<UnavailabilityDto> GetByIdAsync(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<long> CreateAsync(UnavailabilityDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return result;
        }

        [HttpPut]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(UnavailabilityDto dto)
        {
            await _service.UpdateAsync(dto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task DeleteAsync(long id)
        {
            await _service.DeleteByIdAsync(id);
        }
    }
}