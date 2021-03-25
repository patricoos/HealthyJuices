using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Products.DataAccess;
using MediatR;

namespace HealthyJuices.Application.Features.Products.Commands
{
    public class DeleteProduct
    {
        // Command 
        public record Command(string Id) : IRequest { }

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

                product.Remove();

                _productRepository.Update(product);
                await _productRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}