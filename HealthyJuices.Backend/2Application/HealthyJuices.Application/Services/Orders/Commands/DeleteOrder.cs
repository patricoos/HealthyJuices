using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Orders.DataAccess;

namespace HealthyJuices.Application.Services.Orders.Commands
{
    public static class DeleteOrder
    {
        // Command 
        public record Command(string Id) : IRequestWrapper { }

        // Handler
        public class Handler : IHandlerWrapper<Command>
        {
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository repository)
            {
                this._orderRepository = repository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _orderRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return Response.Fail($"Not found order with id: {request.Id}");

                entity.Remove();

                _orderRepository.Update(entity);
                await _orderRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}