using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Features.Unavailabilities.Queries
{
    public abstract class GetAllUnavailabilities
    {
        // Query 
        public record Query(DateTime? From = null, DateTime? To = null) : IRequest<IEnumerable<UnavailabilityDto>> { }

        // Handler
        public class Handler : IRequestHandler<Query, IEnumerable<UnavailabilityDto>>
        {
            private readonly IUnavailabilityRepository _unavailabilityRepository;
            private readonly IMapper _mapper;

            public Handler(IUnavailabilityRepository unavailabilityRepository, IMapper mapper)
            {
                _unavailabilityRepository = unavailabilityRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<UnavailabilityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _unavailabilityRepository.GetAllAsync(request.From, request.To);
                return _mapper.Map<IEnumerable<UnavailabilityDto>>(entities);
            }
        }
    }
}