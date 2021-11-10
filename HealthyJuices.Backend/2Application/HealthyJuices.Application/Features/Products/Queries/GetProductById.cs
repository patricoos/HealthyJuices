using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Shared.Dto.Products;
using MediatR;

namespace HealthyJuices.Application.Features.Products.Queries
{
    public abstract class GetProductById
    {
        // Query 
        public record Query(string Id) : IRequest<ProductDto> { }


        // Handler
        public class Handler : IRequestHandler<Query, ProductDto>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

                public Handler(IProductRepository repository, IMapper mapper)
                {
                    this._productRepository = repository;
                    _mapper = mapper;
                }

            public async Task<ProductDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _productRepository.GetByIdAsync(request.Id);

                if (entity == null)
                    throw new BadRequestException($"Not found product with id: {request.Id}"); 
                
                return _mapper.Map<ProductDto>(entity);
            }
        }
    }
}