using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto.Products;
using MediatR;

namespace HealthyJuices.Application.Functions.Products.Queries
{
    public static class GetProductById
    {
        // Query 
        public record Query(string Id) : IRequest<ProductDto> { }


        // Handler
        public class Handler : IRequestHandler<Query, ProductDto>
        {
        private readonly IProductRepository _productRepository;

            public Handler(IProductRepository repository)
            {
                this._productRepository = repository;
            }

            public async Task<ProductDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _productRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    throw new BadRequestException($"Not found product with id: {request.Id}");

                return entity.ToDto();
            }
        }
    }
}