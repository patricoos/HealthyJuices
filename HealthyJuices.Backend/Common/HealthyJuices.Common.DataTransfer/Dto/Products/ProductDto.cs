using System;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Shared.Dto.Products
{
    public record ProductDto
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }

        public ProductUnitType Unit { get; init; }
        public decimal QuantityPerUnit { get; init; }
        public decimal? DefaultPricePerUnit { get; init; }

        public DateTime DateCreated { get; init; }
        public DateTime DateModified { get; init; }
        public bool IsRemoved { get; init; }
        public bool IsActive { get; set; }
    }
}