using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;

namespace HealthyJuices.Application.Services.Unavailabilities.Commands
{
    public static class DeleteUnavailability
    {
        // Command 
        public record Command(string Id) : IRequestWrapper { }

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
                    return Response.Fail($"Not found unavailability with id: {request.Id}");

                _unavailabilityRepository.Remove(unavailability);
                await _unavailabilityRepository.SaveChangesAsync();

                return Response.Success();
            }
        }
    }
}