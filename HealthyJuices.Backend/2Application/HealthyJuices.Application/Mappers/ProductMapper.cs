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
            DefaultPricePerUnit = e.DefaultPricePerUnit?.ToDto(),
            Created = e.Created.UtcDateTime,
            LastModified = e.LastModified?.UtcDateTime,
            IsRemoved = e.IsRemoved,
            IsActive = e.IsActive
        };

        public static OrderItemDto ToDto(this OrderItem e, bool mapOrder = false) => new OrderItemDto()
        {
            Id = e.Id,
            Amount = e.Amount,
            ProductId = e.ProductId,
            Product = e.Product?.ToDto(),
            Order = mapOrder ? e.Order?.ToDto() : null,
            OrderId = e.OrderId
        };

        public static MoneyDto ToDto(this Money e) => new MoneyDto()
        {
            Amount = e.Amount,
            Currency = e.Currency,
        };
    }
}