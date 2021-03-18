using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Tests.EndToEnd.Extensions;
using HealthyJuices.Tests.EndToEnd.SeedTestData.Abstraction;

namespace HealthyJuices.Tests.EndToEnd.SeedTestData
{
    public class ProductBuilder : TestDataBuilder<Product>
    {
        public static ProductBuilder Create() => new ProductBuilder();

        public ProductBuilder WithName(string name)
        {
            base.Entity.SetProperty(p => p.Name, name);
            return this;
        }

        public ProductBuilder Active()
        {
            base.Entity.SetProperty(p => p.IsActive, true);
            return this;
        }

        public ProductBuilder UnActive()
        {
            base.Entity.SetProperty(p => p.IsActive, false);
            return this;
        }

        public ProductBuilder Removed()
        {
            base.Entity.SetProperty(p => p.IsRemoved, true);
            return this;
        }
    }
}