using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto.Orders;
using MediatR;

namespace HealthyJuices.Application.Features.Orders.Queries
{
    public abstract class GetByIdOrder
    {
        // Query 
        public record Query(string Id) : IRequest<OrderDto> { }

        // Handler
        public class Handler : IRequestHandler<Query, OrderDto>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMapper _mapper;

            public Handler(IOrderRepository repository, IMapper mapper)
            {
                this._orderRepository = repository;
                _mapper = mapper;
            }

            public async Task<OrderDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _orderRepository.GetByIdWithRelations(request.Id);

                if (entity == null)
                    throw new BadRequestException($"Not found order with id: {request.Id}");

                return _mapper.Map<OrderDto>(entity);
            }
        }
    }
}