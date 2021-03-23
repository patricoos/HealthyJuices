using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using MediatR;

namespace HealthyJuices.Application.Functions.Unavailabilities.Commands
{
    public static class DeleteUnavailability
    {
        // Command 
        public record Command(string Id) : IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IUnavailabilityWriteRepository _unavailabilityWriteRepository;

            public Handler(IUnavailabilityWriteRepository writeRepository)
            {
                this._unavailabilityWriteRepository = writeRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var unavailability = await _unavailabilityWriteRepository.GetByIdAsync(request.Id);

                if (unavailability == null)
                    throw new BadRequestException($"Not found unavailability with id: {request.Id}");

                _unavailabilityWriteRepository.Remove(unavailability);
                await _unavailabilityWriteRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}