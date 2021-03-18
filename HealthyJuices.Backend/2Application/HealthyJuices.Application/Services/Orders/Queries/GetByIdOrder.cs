using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto.Orders;

namespace HealthyJuices.Application.Services.Orders.Queries
{
    public static class GetByIdOrder
    {
        // Query 
        public record Query(string Id) : IRequestWrapper<OrderDto> { }

        // Handler
        public class Handler : IHandlerWrapper<Query, OrderDto>
        {
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository repository)
            {
                this._orderRepository = repository;
            }

            public async Task<Response<OrderDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _orderRepository.Query()
                    .ById(request.Id)
                    .IncludeDestinationCompany()
                    .IncludeProducts()
                        .FirstOrDefaultAsync();

                if (entity == null)
                    return Response<OrderDto>.Fail<OrderDto>($"Not found order with id: {request.Id}");

                return Response<OrderDto>.Success(entity.ToDto());
            }
        }
    }
}