using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Mappers
{
    public static class UnavailabilityMapper
    {
        public static UnavailabilityDto ToDto(this Unavailability e) => new UnavailabilityDto()
        {
            Id = e.Id,
            From = e.From,
            To = e.To,
            Reason = e.Reason,
            Comment = e.Comment
        };
    }
}