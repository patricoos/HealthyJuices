using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Shared.Dto.Orders;

namespace HealthyJuices.Application.Services.Orders.Commands
{
    public static class UpdateOrder
    {
        // Command 
        public record Command : OrderDto, IRequestWrapper { }

        // Handler
        public class Handler : IHandlerWrapper<Command>
        {
            private readonly IOrderRepository _companyRepository;

            public Handler(IOrderRepository repository)
            {
                this._companyRepository = repository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _companyRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return Response.Fail($"Not found order with id: {request.Id}");

                entity.Update(request.DeliveryDate);

                _companyRepository.Update(entity);
                await _companyRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}