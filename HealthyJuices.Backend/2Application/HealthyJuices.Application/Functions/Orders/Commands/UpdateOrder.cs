using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto.Orders;
using MediatR;

namespace HealthyJuices.Application.Functions.Orders.Commands
{
    public static class UpdateOrder
    {
        // Command 
        public record Command : OrderDto, IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IOrderRepository _companyRepository;

            public Handler(IOrderRepository repository)
            {
                this._companyRepository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _companyRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    throw new BadRequestException($"Not found order with id: {request.Id}");

                entity.Update(request.DeliveryDate);

                _companyRepository.Update(entity);
                await _companyRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}