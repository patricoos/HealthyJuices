using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Application.Services.Products.Commands
{
    public static class UpdateProduct
    {
        // Command 
        public record Command : ProductDto, IRequestWrapper { }

        // Handler
        public class Handler : IHandlerWrapper<Command>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository repository)
            {
                this._productRepository = repository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (product == null)
                    return Response.Fail($"Not found product with id: {request.Id}");

                product.Update(request.Name, request.Description, request.Unit, request.QuantityPerUnit, request.IsActive, request.DefaultPricePerUnit?.Amount);

                _productRepository.Update(product);
                await _productRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}