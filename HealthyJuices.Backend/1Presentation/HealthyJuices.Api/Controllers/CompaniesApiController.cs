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
    [Route("companies")]
    public class CompaniesApiController : ApiController
    {
        private readonly CompaniesService _service;

        public CompaniesApiController(CompaniesService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<List<CompanyDto>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return result;
        }

        [HttpGet("active")]
        public async Task<List<CompanyDto>> GetAllActiveAsync()
        {
            var result = await _service.GetAllActiveAsync();
            return result;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<CompanyDto> GetByIdAsync(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        [Authorize]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<long> CreateAsync(CompanyDto definitionDto)
        {
            var result = await _service.CreateAsync(definitionDto);
            return result;
        }

        [HttpPut]
        [Authorize]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(CompanyDto definitionDto)
        {
            await _service.UpdateAsync(definitionDto);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task DeleteAsync(long id)
        {
            await _service.DeleteByIdAsync(id);
        }
    }
}