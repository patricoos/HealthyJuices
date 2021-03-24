using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Functions.Unavailabilities.Commands
{
    public static class CreateUnavailability
    {
        // Command 
        public record Command : UnavailabilityDto, IRequest<string> { }

        // Handler
        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IUnavailabilityRepository _unavailabilityRepository;

            public Handler(IUnavailabilityRepository repository)
            {
                this._unavailabilityRepository = repository;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var unavailability = new Unavailability(request.From, request.To, request.Reason, request.Comment);

                _unavailabilityRepository.Insert(unavailability);
                await _unavailabilityRepository.SaveChangesAsync();

                return unavailability.Id;
            }
        }
    }
}