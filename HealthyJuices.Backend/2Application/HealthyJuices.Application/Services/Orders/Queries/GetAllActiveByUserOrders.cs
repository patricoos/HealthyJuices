using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto.Orders;
using MediatR;

namespace HealthyJuices.Application.Services.Orders.Queries
{
    public static class GetAllActiveByUserOrders
    {
        // Query 
        public record Query(string userId, DateTime? from, DateTime? to) : IRequest<IEnumerable<OrderDto>> { }

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
                var query = _orderRepository.Query()
                    .IncludeProducts()
                    .IsNotRemoved()
                    .ByUser(request.userId);

                if (request.from.HasValue)
                    query.AfterDateTime(request.from.Value);

                if (request.to.HasValue)
                    query.BeforeDateTime(request.to.Value);

                var entities = await query.ToListAsync();

                var result = entities
                    .Select(x => x.ToDto())
                    .ToList();

                return result;
            }
        }
    }
}