using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Api.Utils.Attributes;
using HealthyJuices.Application.Services.Products.Commands;
using HealthyJuices.Application.Services.Products.Queries;
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
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var response = await _mediator.Send(new GetProductById.Query(id));
            return response.Failed ? BadRequest(response.Message) : Ok(response.Value);
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<IActionResult> CreateAsync(CreateProduct.Command command)
        {
            var response = await _mediator.Send(command);
            return response.Failed ? BadRequest(response.Message) : Ok(response.Value);
        }

        [HttpPut]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<IActionResult> UpdateAsync(UpdateProduct.Command command)
        {
            var response = await _mediator.Send(command);
            return response.Failed ? BadRequest(response.Message) : Ok();
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.BusinessOwner)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var response = await _mediator.Send(new DeleteProduct.Command(id));
            return response.Failed ? BadRequest(response.Message) : Ok();
        }
    }
}