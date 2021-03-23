using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using MediatR;

namespace HealthyJuices.Application.Functions.Orders.Commands
{
    public static class DeleteOrder
    {
        // Command 
        public record Command(string Id) : IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository repository)
            {
                this._orderRepository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _orderRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    throw new BadRequestException($"Not found order with id: {request.Id}");

                entity.Remove();

                _orderRepository.Update(entity);
                await _orderRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}