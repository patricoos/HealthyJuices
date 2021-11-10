using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto.Orders;
using MediatR;

namespace HealthyJuices.Application.Features.Orders.Queries
{
    public abstract class GetAllActiveByUserOrders
    {
        // Query 
        public record Query(string UserId, DateTime? From = null, DateTime? To = null) : IRequest<IEnumerable<OrderDto>> { }

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
                var entities = await _orderRepository.GetByUserIdAndDatesWithProductsAsync(request.UserId, request.From, request.To);
                return _mapper.Map<IEnumerable<OrderDto>>(entities);
            }
        }
    }
}