using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Shared.Dto.Products;

namespace HealthyJuices.Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(this Product e) => new ProductDto()
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Unit = e.Unit,
            QuantityPerUnit = e.QuantityPerUnit,
            DefaultPricePerUnit = e.DefaultPricePerUnit,
            DateCreated = e.DateCreated,
            DateModified = e.DateModified,
            IsRemoved = e.IsRemoved,
        };
    }
}