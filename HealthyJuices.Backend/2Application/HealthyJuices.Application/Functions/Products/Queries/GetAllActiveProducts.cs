using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
            private readonly IMapper _mapper;

            public Handler(IProductRepository repository, IMapper mapper)
            {
                this._productRepository = repository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _productRepository.GetAllActiveAsync();
                return _mapper.Map<IEnumerable<ProductDto>>(entities);
            }
        }
    }
}