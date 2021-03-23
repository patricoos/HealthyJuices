using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto.Orders;
using MediatR;

namespace HealthyJuices.Application.Functions.Orders.Queries
{
    public static class GetAllActiveOrders
    {
        // Query 
        public record Query(DateTime? From = null, DateTime? To = null) : IRequest<IEnumerable<OrderDto>> { }

        // Handler
        public class Handler : IRequestHandler<Query, IEnumerable<OrderDto>>
        {
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository repository)
            {
                this._orderRepository = repository;
            }

            public async Task<IEnumerable<OrderDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _orderRepository.GetAllActiveWithUserAndDestinationCompanyAsync(request.From, request.To);

                var result = entities
                    .Select(x => x.ToDto())
                    .ToList();

                return result;
            }
        }
    }
}