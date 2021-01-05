using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Products;

namespace HealthyJuices.Domain.Models.Orders
{
    public class OrderProduct: Entity
    {
        public long OrderId { get; set; }
        public Order Order { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        public decimal Amount { get; set; }

        public OrderProduct() { }

        public OrderProduct(Order order, Product product, decimal amount)
        {
            this.Order = order;
            this.Product = product.IsRemoved || !product.IsActive ? throw new ConflictException($"{nameof(Product)} {product.Name} is not active"):  product;
            this.Amount = amount;
        }
    }
}