using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Application.Services.Products.Queries
{
    public static class GetProductById
    {
        // Query 
        public record Query(string Id) : IRequestWrapper<ProductDto> { }


        // Handler
        public class Handler : IHandlerWrapper<Query, ProductDto>
        {
        private readonly IProductRepository _productRepository;

            public Handler(IProductRepository repository)
            {
                this._productRepository = repository;
            }

            public async Task<Response<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _productRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return Response<ProductDto>.Fail<ProductDto>($"Not found product with id: {request.Id}");

                return Response<ProductDto>.Success(entity.ToDto());
            }
        }
    }
}