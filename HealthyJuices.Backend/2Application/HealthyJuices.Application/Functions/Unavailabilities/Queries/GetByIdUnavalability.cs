﻿using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Mappers;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Shared.Dto;
using MediatR;

namespace HealthyJuices.Application.Functions.Unavailabilities.Queries
{
    public static class GetByIdUnavalability
    {
        // Query 
        public record Query(string Id) : IRequest<UnavailabilityDto> { }

        // Handler
        public class Handler : IRequestHandler<Query, UnavailabilityDto>
        {
            private readonly IUnavailabilityRepository _unavailabilityRepository;

            public Handler(IUnavailabilityRepository unavailabilityRepository)
            {
                _unavailabilityRepository = unavailabilityRepository;
            }

            public async Task<UnavailabilityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _unavailabilityRepository.GetByIdAsync(request.Id);

                if (entity == null)
                    throw new BadRequestException($"Not found unavalability with id: {request.Id}");

                return entity.ToDto();
            }
        }
    }
}