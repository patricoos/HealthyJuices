using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto.Products;
using MediatR;

namespace HealthyJuices.Application.Features.Products.Commands
{
    public static class CreateProduct
    {
        // Command 
        public record Command : ProductDto, IRequest<string> { }

        // Validator
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Names is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

                RuleFor(v => v.Unit)
                    .NotNull().WithMessage("Unit is required.");
            }
        }

        // Handler
        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository repository)
            {
                this._productRepository = repository;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = new Product(request.Name, request.Description, request.Unit, request.QuantityPerUnit, request.IsActive, request.DefaultPricePerUnit?.Amount);

                _productRepository.Insert(product);
                await _productRepository.SaveChangesAsync();

                return product.Id;
            }
        }
    }
}