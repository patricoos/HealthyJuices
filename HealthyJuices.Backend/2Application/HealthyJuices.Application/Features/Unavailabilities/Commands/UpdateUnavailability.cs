using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Features.Unavailabilities.Commands
{
    public static class UpdateUnavailability
    {
        // Command 
        public record Command : UnavailabilityDto, IRequest { }

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
                    throw new BadRequestException($"Not found product with id: {request.Id}");

                unavailability.Update(request.From, request.To, request.Reason, request.Comment);

                _unavailabilityRepository.Update(unavailability);
                await _unavailabilityRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}