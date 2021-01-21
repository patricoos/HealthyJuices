﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Services;
using HealthyJuices.Shared.Dto.Products;
using HealthyJuices.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
    [Route("products")]
    [Authorize]
    public class ProductsApiController : ApiController
    {
        private readonly ProductsService _service;

        public ProductsApiController(ProductsService service)
        {
            _service = service;
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return result;
        }

        [HttpGet("active")]
        public async Task<List<ProductDto>> GetAllActiveAsync()
        {
            var result = await _service.GetAllActiveAsync();
            return result;
        }


        [HttpGet("{id}")]
        public async Task<ProductDto> GetByIdAsync(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<long> CreateAsync(ProductDto definitionDto)
        {
            var result = await _service.CreateAsync(definitionDto);
            return result;
        }

        [HttpPut]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(ProductDto definitionDto)
        {
            await _service.UpdateAsync(definitionDto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task DeleteAsync(long id)
        {
            await _service.DeleteByIdAsync(id);
        }
    }
}