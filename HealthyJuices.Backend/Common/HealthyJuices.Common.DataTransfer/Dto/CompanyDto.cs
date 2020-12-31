using System;

namespace HealthyJuices.Shared.Dto
{
    public record CompanyDto
    {
        public long Id{ get; set; }
        public string Name { get; init; }
        public string Comment { get; init; }

        public string PostalCode { get; init; }
        public string City { get; init; }
        public string Street { get; init; }

        public double Latitude { get; init; }
        public double Longitude { get; init; }

        public bool IsRemoved { get; init; }
        public DateTime DateCreated { get; init; }
        public DateTime DateModified { get; init; }

    }
}