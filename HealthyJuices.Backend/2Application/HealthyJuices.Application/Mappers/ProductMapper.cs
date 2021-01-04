using HealthyJuices.Domain.Models.Orders;
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

        public static OrderProductDto ToDto(this OrderProduct e, bool mapOrder = false) => new OrderProductDto()
        {
            Id = e.Id,
            Amount = e.Amount,
            ProductId = e.ProductId,
            Product = e.Product?.ToDto(),
            Order = mapOrder ? e.Order?.ToDto() : null,
            OrderId = e.OrderId
        };
    }
}