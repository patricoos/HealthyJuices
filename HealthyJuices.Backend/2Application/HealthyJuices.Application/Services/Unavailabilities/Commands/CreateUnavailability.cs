using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Services.Unavailabilities.Commands
{
    public static class CreateUnavailability
    {
        // Command 
        public record Command : UnavailabilityDto, IRequestWrapper<string> { }

        // Handler
        public class Handler : IHandlerWrapper<Command, string>
        {
            private readonly IUnavailabilityRepository _unavailabilityRepository;

            public Handler(IUnavailabilityRepository repository)
            {
                this._unavailabilityRepository = repository;
            }

            public async Task<Response<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var unavailability = new Unavailability(request.From, request.To, request.Reason, request.Comment);

                _unavailabilityRepository.Insert(unavailability);
                await _unavailabilityRepository.SaveChangesAsync();

                return Response<string>.Success(unavailability.Id);
            }
        }
    }
}