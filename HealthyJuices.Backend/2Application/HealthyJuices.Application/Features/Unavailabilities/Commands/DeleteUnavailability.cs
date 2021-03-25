using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using MediatR;

namespace HealthyJuices.Application.Features.Unavailabilities.Commands
{
    public static class DeleteUnavailability
    {
        // Command 
        public record Command(string Id) : IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IUnavailabilityRepository _unavailabilityRepository;

            public Handler(IUnavailabilityRepository repository)
            {
                this._unavailabilityRepository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var unavailability = await _unavailabilityRepository.GetByIdAsync(request.Id);

                if (unavailability == null)
                    throw new BadRequestException($"Not found unavailability with id: {request.Id}");

                _unavailabilityRepository.Remove(unavailability);
                await _unavailabilityRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}