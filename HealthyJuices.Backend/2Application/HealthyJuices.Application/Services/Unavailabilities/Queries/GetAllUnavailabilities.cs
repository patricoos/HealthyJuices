using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Services.Unavailabilities.Queries
{
    public static class GetAllUnavailabilities
    {
        // Query 
        public record Query(DateTime? From = null, DateTime? To = null) : IRequest<IEnumerable<UnavailabilityDto>> { }

        // Handler
        public class Handler : IRequestHandler<Query, IEnumerable<UnavailabilityDto>>
        {
            private readonly IUnavailabilityRepository _unavailabilityRepository;

            public Handler(IUnavailabilityRepository unavailabilityRepository)
            {
                _unavailabilityRepository = unavailabilityRepository;
            }

            public async Task<IEnumerable<UnavailabilityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _unavailabilityRepository.Query();
                if (request.From.HasValue)
                    query.AfterDateTime(request.From.Value);

                if (request.To.HasValue)
                    query.BeforeDateTime(request.To.Value);

                var entities = await query.ToListAsync();

                var result = entities
                    .Select(x => x.ToDto())
                    .ToList();

                return result;
            }
        }
    }
}