using System;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Shared.Dto
{
    public record UnavailabilityDto
    {
        public long? Id { get; init; }

        public DateTime From { get; init; }
        public DateTime To { get; init; }

        public UnavailabilityReason Reason { get; init; }

        public string Comment { get; init; }
    }
}