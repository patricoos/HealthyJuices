using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Services;
using HealthyJuices.Application.Services.Orders.Commands;
using HealthyJuices.Application.Services.Orders.Queries;
using HealthyJuices.Shared.Dto.Orders;
using HealthyJuices.Shared.Dto.Reports;
using HealthyJuices.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
    [Route("orders")]
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var result = await _mediator.Send(new GetAllOrders.Query());
            return result;
        }

        [HttpGet("my")]
        [AuthorizeRoles(UserRole.Customer)]
        public async Task<IEnumerable<OrderDto>> GetAllActiveByUserAsync([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var result = await _mediator.Send(new GetAllActiveByUserOrders.Query(RequestSenderId, from, to));
            return result;
        }

        [HttpGet("active")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<IEnumerable<OrderDto>> GetAllActiveAsync([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var result = await _mediator.Send(new GetAllActiveOrders.Query(from, to));
            return result;
        }

        [HttpGet("{id}")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<OrderDto> GetByIdAsync(string id)
        {
            var response = await _mediator.Send(new GetByIdOrder.Query(id));
            return response;
        }

        //[HttpPost]
        //[AuthorizeRoles(UserRole.Customer)]
        //public async Task<string> CreateAsync(CreateOrder.Command command)
        //{
        //    var response = await _mediator.Send(command);
        //    return response;
        //}

        //[HttpPut]
        //[AuthorizeRoles(UserRole.BusinessOwner)]
        //public async Task UpdateAsync(UpdateOrder.Command command)
        //{
        //    await _mediator.Send(command);
        //}

        //[HttpDelete("{id}")]
        //[AuthorizeRoles(UserRole.BusinessOwner)]
        //public async Task DeleteAsync(string id)
        //{
        //    await _mediator.Send(new DeleteOrder.Command(id));
        //}


        [HttpGet("dashboard-report")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<DashboardOrderReportDto> GetDashboardOrderReportAsync([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _mediator.Send(new GetDashboardReportOrders.Query(from, to));
            return result;
        }
    }
}