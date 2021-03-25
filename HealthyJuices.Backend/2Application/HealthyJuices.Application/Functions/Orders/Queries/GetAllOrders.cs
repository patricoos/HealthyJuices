using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto.Orders;
using MediatR;

namespace HealthyJuices.Application.Functions.Orders.Queries
{
    public static class GetAllOrders
    {
        // Query 
        public record Query : IRequest<IEnumerable<OrderDto>> { }

        // Handler
        public class Handler : IRequestHandler<Query, IEnumerable<OrderDto>>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMapper _mapper;

            public Handler(IOrderRepository repository, IMapper mapper)
            {
                this._orderRepository = repository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<OrderDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _orderRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<OrderDto>>(entities);
            }
        }
    }
}