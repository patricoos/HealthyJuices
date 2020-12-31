﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Controllers;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Dto.Products;
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
        private readonly OrdersController _appController;

        public OrdersApiController(OrdersController appController)
        {
            _appController = appController;
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<List<OrderDto>> GetAllAsync()
        {
            var result = await _appController.GetAllAsync();
            return result;
        }

        [HttpGet("active")]
        public async Task<List<OrderDto>> GetAllActiveAsync()
        {
            var result = await _appController.GetAllActiveAsync();
            return result;
        }


        [HttpGet("{id}")]
        public async Task<OrderDto> GetByIdAsync(long id)
        {
            var result = await _appController.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<long> CreateAsync(OrderDto definitionDto)
        {
            var result = await _appController.CreateAsync(definitionDto);
            return result;
        }

        [HttpPut]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(OrderDto definitionDto)
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