using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto.Products;
using MediatR;

namespace HealthyJuices.Application.Functions.Products.Queries
{
    public static class GetAllActiveProducts
    {
        // Query 
        public record Query : IRequest<IEnumerable<ProductDto>> {}

        // Handler
        public class Handler : IRequestHandler<Query, IEnumerable<ProductDto>>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository repository)
            {
                this._productRepository = repository;
            }

            public async Task<IEnumerable<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _productRepository.Query()
                    .IsActive()
                    .IsNotRemoved()
                    .ToListAsync();

                var result = entities
                    .Select(x => x.ToDto())
                    .ToList();

                return result;
            }
        }
    }
}