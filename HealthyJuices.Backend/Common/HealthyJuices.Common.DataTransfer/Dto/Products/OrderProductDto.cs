using HealthyJuices.Shared.Dto.Orders;

namespace HealthyJuices.Shared.Dto.Products
{
    public record OrderItemDto
    {
        public string Id { get; init; }
        public string OrderId { get; init; }
        public OrderDto Order { get; init; }

        public string ProductId { get; init; }
        public ProductDto Product { get; init; }

        public decimal Amount { get; init; }

    }
}