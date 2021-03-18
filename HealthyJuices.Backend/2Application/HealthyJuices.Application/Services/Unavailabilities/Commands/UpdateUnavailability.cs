using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Services.Unavailabilities.Commands
{
    public static class UpdateUnavailability
    {
        // Command 
        public record Command : UnavailabilityDto, IRequestWrapper { }

        // Handler
        public class Handler : IHandlerWrapper<Command>
        {
            private readonly IUnavailabilityRepository _unavailabilityRepository;

            public Handler(IUnavailabilityRepository repository)
            {
                this._unavailabilityRepository = repository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var unavailability = await _unavailabilityRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (unavailability == null)
                   Response.Fail($"Not found product with id: {request.Id}");

                unavailability.Update(request.From, request.To, request.Reason, request.Comment);

                _unavailabilityRepository.Update(unavailability);
                await _unavailabilityRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}