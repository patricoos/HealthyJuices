using System;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Shared.Dto.Products
{
    public record ProductDto
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }

        public ProductUnitType Unit { get; init; }
        public decimal QuantityPerUnit { get; init; }
        public MoneyDto DefaultPricePerUnit { get; init; }

        public DateTime Created { get; init; }
        public DateTime?LastModified { get; init; }
        public bool IsRemoved { get; init; }
        public bool IsActive { get; set; }
    }
}