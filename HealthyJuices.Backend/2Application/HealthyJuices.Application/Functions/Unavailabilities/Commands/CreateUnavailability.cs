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
            private readonly IUnavailabilityWriteRepository _unavailabilityWriteRepository;

            public Handler(IUnavailabilityWriteRepository writeRepository)
            {
                this._unavailabilityWriteRepository = writeRepository;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var unavailability = new Unavailability(request.From, request.To, request.Reason, request.Comment);

                _unavailabilityWriteRepository.Insert(unavailability);
                await _unavailabilityWriteRepository.SaveChangesAsync();

                return unavailability.Id;
            }
        }
    }
}