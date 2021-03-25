using System;

namespace HealthyJuices.Shared.Dto
{
    public record CompanyDto
    {
        public string Id{ get; set; }
        public string Name { get; init; }
        public string Comment { get; init; }

        public string PostalCode { get; init; }
        public string City { get; init; }
        public string Street { get; init; }

        public double Latitude { get; init; }
        public double Longitude { get; init; }

        public bool IsRemoved { get; init; }
        public DateTime Created { get; init; }
        public DateTime? LastModified { get; init; }

    }
}