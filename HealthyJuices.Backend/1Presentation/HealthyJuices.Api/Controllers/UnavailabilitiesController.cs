using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Features.Unavailabilities.Commands;
using HealthyJuices.Application.Features.Unavailabilities.Queries;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
    [Route("unavailabilities")]
    [Authorize]
    public class UnavailabilitiesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UnavailabilitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<UnavailabilityDto>> GetAllAsync([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var result = await _mediator.Send(new GetAllUnavailabilities.Query(from, to));
            return result;
        }

        [HttpGet("{id}")]
        public async Task<UnavailabilityDto> GetByIdAsync(string id)
        {
            var response = await _mediator.Send(new GetByIdUnavalability.Query(id));
            return response;
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<string> CreateAsync(CreateUnavailability.Command command)
        {
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpPut]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(UpdateUnavailability.Command command)
        {
            await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task DeleteAsync(string id)
        {
            await _mediator.Send(new DeleteUnavailability.Command(id));
        }
    }
}