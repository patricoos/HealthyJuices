using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Products;

namespace HealthyJuices.Domain.Models.Orders
{
    public class OrderProduct: Entity
    {
        public string OrderId { get; set; }
        public Order Order { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public decimal Amount { get; set; }

        protected OrderProduct() { }

        public OrderProduct(Order order, Product product, decimal amount)
        {
            this.Order = order;
            this.Amount = amount <= 0 ? throw new BadRequestException("Amount must be greater than 0") : amount;
            this.SetProduct(product);
        }

        private void SetProduct(Product product)
        {
            if (product == null || product.IsRemoved)
                throw new BadRequestException("Product not exists");

            if (!product.IsActive)
                throw new BadRequestException($"Product '{product.Name}' is not active");

            this.Product = product;
        }
    }
}