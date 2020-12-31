﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Controllers;
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
        private readonly ProductsController _appController;

        public ProductsApiController(ProductsController appController)
        {
            _appController = appController;
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var result = await _appController.GetAllAsync();
            return result;
        }

        [HttpGet("active")]
        public async Task<List<ProductDto>> GetAllActiveAsync()
        {
            var result = await _appController.GetAllActiveAsync();
            return result;
        }


        [HttpGet("{id}")]
        public async Task<ProductDto> GetByIdAsync(long id)
        {
            var result = await _appController.GetByIdAsync(id);
            return result;
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<long> CreateAsync(ProductDto definitionDto)
        {
            var result = await _appController.CreateAsync(definitionDto);
            return result;
        }

        [HttpPut]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(ProductDto definitionDto)
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