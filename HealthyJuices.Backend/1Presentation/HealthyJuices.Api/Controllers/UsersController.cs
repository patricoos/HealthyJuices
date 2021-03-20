using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Services;
using HealthyJuices.Application.Services.Orders.Queries;
using HealthyJuices.Application.Services.Users.Commands;
using HealthyJuices.Application.Services.Users.Queries;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
    [Authorize]
    [AuthorizeRoles(UserRole.BusinessOwner)]
    [Route("users")]
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var result = await _mediator.Send(new GetAllUsers.Query());
            return result;
        }

        [HttpGet("active")]
        public async Task<IEnumerable<UserDto>> GetAllActiveAsync()
        {
            var response = await _mediator.Send(new GetAllActiveUsers.Query());
            return response;
        }

        [HttpGet("role/{role}")]
        public async Task<IEnumerable<UserDto>> GetAllActiveByUserRoleAsync(UserRole role)
        {
            var response = await _mediator.Send(new GetAllActiveByUserRoleUsers.Query(role));
            return response;
        }

        [HttpGet("{id}")]
        public async Task<UserDto> GetByIdAsync(string id)
        {
            var response = await _mediator.Send(new GetByIdUser.Query(id));
            return response;
        }

        [HttpGet("{email}")]
        public async Task<UserDto> GetAsync(string email)
        {
            var response = await _mediator.Send(new GetByEmailUser.Query(email));
            return response;
        }

        [AllowAnonymous]
        [HttpGet("existing/{userName}")]
        public async Task<bool> IsExistingAsync(string userName)
        {
            var response = await _mediator.Send(new IsExistingUser.Query(userName));
            return response;
        }

        [HttpPost]
        public async Task<string> CreateAsync([FromBody] CreateUser.Command command)
        {
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _mediator.Send(new DeleteUser.Command(id));
        }
    }
}