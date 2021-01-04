using HealthyJuices.Shared.Dto.Orders;

namespace HealthyJuices.Shared.Dto.Products
{
    public record OrderProductDto
    {
        public long Id { get; init; }
        public long OrderId { get; init; }
        public OrderDto Order { get; init; }

        public long ProductId { get; init; }
        public ProductDto Product { get; init; }

        public decimal Amount { get; init; }

    }
}