using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Application.Wrappers;
using HealthyJuices.Common.Utils;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Services.Unavailabilities.Queries
{
    public static class GetByIdUnavalability
    {
        // Query 
        public record Query(string Id) : IRequestWrapper<UnavailabilityDto> { }

        // Handler
        public class Handler : IHandlerWrapper<Query, UnavailabilityDto>
        {
            private readonly IUnavailabilityRepository _unavailabilityRepository;

            public Handler(IUnavailabilityRepository unavailabilityRepository)
            {
                _unavailabilityRepository = unavailabilityRepository;
            }

            public async Task<Response<UnavailabilityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _unavailabilityRepository.Query()
                    .ById(request.Id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return Response<UnavailabilityDto>.Fail<UnavailabilityDto>($"Not found unavalability with id: {request.Id}");

                return Response<UnavailabilityDto>.Success(entity.ToDto());
            }
        }
    }
}