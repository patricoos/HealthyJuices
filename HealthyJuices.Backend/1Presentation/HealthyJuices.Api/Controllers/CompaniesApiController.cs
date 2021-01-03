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
    [Route("companies")]
    [Authorize]
    public class CompaniesApiController : ApiController
    {
        private readonly CompaniesController _appController;

        public CompaniesApiController(CompaniesController appController)
        {
            _appController = appController;
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<List<CompanyDto>> GetAllAsync()
        {
            var result = await _appController.GetAllAsync();
            return result;
        }

        [HttpGet("active")]
        public async Task<List<CompanyDto>> GetAllActiveAsync()
        {
            var result = await _appController.GetAllActiveAsync();
            return result;
        }


        [HttpGet("{id}")]
        public async Task<CompanyDto> GetByIdAsync(long id)
        {
            var result = await _appController.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<long> CreateAsync(CompanyDto definitionDto)
        {
            var result = await _appController.CreateAsync(definitionDto);
            return result;
        }

        [HttpPut]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(CompanyDto definitionDto)
        {
            await _appController.UpdateAsync(definitionDto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task DeleteAsync(long id)
        {
            await _appController.DeleteByIdAsync(id);
        }
    }
}