using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Features.Companies.Commands;
using HealthyJuices.Application.Features.Companies.Queries;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
    [Route("companies")]
    public class CompaniesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<IEnumerable<CompanyDto>> GetAllAsync()
        {
            var result = await _mediator.Send(new GetAllCompanies.Query());
            return result;
        }

        [HttpGet("active")]
        public async Task<IEnumerable<CompanyDto>> GetAllActiveAsync()
        {
            var result = await _mediator.Send(new GetAllActiveCompanies.Query());
            return result;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<CompanyDto> GetByIdAsync(string id)
        {
            var response = await _mediator.Send(new GetByIdCompany.Query(id));
            return response;
        }

        [HttpPost]
        [Authorize]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<string> CreateAsync(CreateCompany.Command command)
        {
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpPut]
        [Authorize]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(UpdateCompany.Command command)
        {
            await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task DeleteAsync(string id)
        {
            await _mediator.Send(new DeleteCompany.Command(id));
        }
    }
}