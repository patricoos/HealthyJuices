using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Application.Services.Products.Commands
{
    public static class CreateProduct
    {
        // Command 
        public record Command : ProductDto, IRequestWrapper<string> { }

        // Handler
        public class Handler : IHandlerWrapper<Command, string>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository repository)
            {
                this._productRepository = repository;
            }

            public async Task<Response<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = new Product(request.Name, request.Description, request.Unit, request.QuantityPerUnit, request.IsActive, request.DefaultPricePerUnit?.Amount);

                _productRepository.Insert(product);
                await _productRepository.SaveChangesAsync();

                return Response<string>.Success(product.Id);
            }
        }
    }
}