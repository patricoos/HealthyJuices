﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Features.Products.Commands;
using HealthyJuices.Application.Features.Products.Queries;
using HealthyJuices.Shared.Dto.Products;
using HealthyJuices.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyJuices.Api.Controllers
{
    [ApiController]
    [Route("products")]
    [Authorize]
    public class ProductsController : BaseApiController
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var result = await _mediator.Send(new GetAllProducts.Query());
            return result;
        }

        [HttpGet("active")]
        public async Task<IEnumerable<ProductDto>> GetAllActiveAsync()
        {
            var result = await _mediator.Send(new GetAllActiveProducts.Query());
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ProductDto> GetByIdAsync(string id)
        {
            var response = await _mediator.Send(new GetProductById.Query(id));
            return response;
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<string> CreateAsync(CreateProduct.Command command)
        {
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpPut]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task UpdateAsync(UpdateProduct.Command command)
        {
            await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task DeleteAsync(string id)
        {
            await _mediator.Send(new DeleteProduct.Command(id));
        }
    }
}