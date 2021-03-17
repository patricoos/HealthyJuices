using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Products.DataAccess;

namespace HealthyJuices.Application.Services.Products.Commands
{
    public class DeleteProduct
    {
        // Command 
        public record Command(string Id) : IRequestWrapper { }

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

                product.Remove();

                _productRepository.Update(product);
                await _productRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}