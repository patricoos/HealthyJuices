using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Functions.Unavailabilities.Commands
{
    public static class UpdateUnavailability
    {
        // Command 
        public record Command : UnavailabilityDto, IRequest { }

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
                    throw new BadRequestException($"Not found product with id: {request.Id}");

                unavailability.Update(request.From, request.To, request.Reason, request.Comment);

                _unavailabilityWriteRepository.Update(unavailability);
                await _unavailabilityWriteRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}