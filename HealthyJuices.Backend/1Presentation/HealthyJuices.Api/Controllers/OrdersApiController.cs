using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Services;
using HealthyJuices.Shared.Dto.Orders;
using HealthyJuices.Shared.Dto.Reports;
using HealthyJuices.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
    [Route("orders")]
    [Authorize]
    public class OrdersApiController : ApiController
    {
        private readonly OrdersService _service;

        public OrdersApiController(OrdersService service)
        {
            _service = service;
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<List<OrderDto>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return result;
        }

        [HttpGet("my")]
        [AuthorizeRoles(UserRole.Customer)]
        public async Task<List<OrderDto>> GetAllActiveByUserAsync([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var result = await _service.GetAllActiveByUserAsync(RequestSenderId, from, to);
            return result;
        }

        [HttpGet("active")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<List<OrderDto>> GetAllActiveAsync([FromQuery]DateTime? from = null, [FromQuery]DateTime? to = null)
        {
            var result = await _service.GetAllActiveAsync(from, to);
            return result;
        }

        [HttpGet("{id}")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<OrderDto> GetByIdAsync(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.Customer)]
        public async Task<long> CreateAsync(OrderDto definitionDto)
        {
            var result = await _service.CreateAsync(definitionDto,RequestSenderId);
            return result;
        }

        [HttpPut]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(OrderDto definitionDto)
        {
            await _service.UpdateAsync(definitionDto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task DeleteAsync(long id)
        {
            await _service.DeleteByIdAsync(id);
        }


        [HttpGet("dashboard-report")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<DashboardOrderReportDto> GetDashboardOrderReportAsync([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _service.GetDashboardOrderReportAsync(from, to);
            return result;
        }
    }
}