using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Mappers
{
    public static class CompanyMapper
    {
        public static CompanyDto ToDto(this Company e) => new CompanyDto()
        {
            Id = e.Id,
            Name = e.Name,

            Comment = e.Comment,
            PostalCode = e.PostalCode,
            City = e.City,
            Street = e.Street,

            Latitude = e.Latitude,
            Longitude = e.Longitude,

            DateCreated = e.DateCreated.UtcDateTime,
            DateModified = e.DateModified.UtcDateTime,
            IsRemoved = e.IsRemoved,
        };
    }
}