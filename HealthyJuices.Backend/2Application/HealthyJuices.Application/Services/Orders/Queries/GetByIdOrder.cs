using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto.Orders;
using MediatR;

namespace HealthyJuices.Application.Services.Orders.Queries
{
    public static class GetByIdOrder
    {
        // Query 
        public record Query(string Id) : IRequest<OrderDto> { }

        // Handler
        public class Handler : IRequestHandler<Query, OrderDto>
        {
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository repository)
            {
                this._orderRepository = repository;
            }

            public async Task<OrderDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _orderRepository.Query()
                    .ById(request.Id)
                    .IncludeDestinationCompany()
                    .IncludeProducts()
                    .FirstOrDefaultAsync();

                if (entity == null)
                    throw new BadRequestException($"Not found order with id: {request.Id}");

                return entity.ToDto();
            }
        }
    }
}