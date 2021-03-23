using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto.Products;
using MediatR;

namespace HealthyJuices.Application.Functions.Products.Commands
{
    public static class UpdateProduct
    {
        // Command 
        public record Command : ProductDto, IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository repository)
            {
                this._productRepository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(request.Id);

                if (product == null)
                    throw new BadRequestException($"Not found product with id: {request.Id}");

                product.Update(request.Name, request.Description, request.Unit, request.QuantityPerUnit, request.IsActive, request.DefaultPricePerUnit?.Amount);

                _productRepository.Update(product);
                await _productRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}