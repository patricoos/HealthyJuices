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
    public abstract class GetAllActiveOrders
    {
        // Query 
        public record Query(DateTime? From = null, DateTime? To = null) : IRequest<IEnumerable<OrderDto>> { }

        // Handler
        public class Handler : IRequestHandler<Query, IEnumerable<OrderDto>>
        {
            private readonly IMapper _mapper;
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository repository, IMapper mapper)
            {
                this._orderRepository = repository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<OrderDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _orderRepository.GetAllActiveWithUserAndDestinationCompanyAsync(request.From, request.To);
                return _mapper.Map<IEnumerable<OrderDto>>(entities);
            }
        }
    }
}