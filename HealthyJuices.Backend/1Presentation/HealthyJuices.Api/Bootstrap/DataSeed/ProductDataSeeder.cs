using System.Collections.Generic;
using System.Linq;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Api.Bootstrap.DataSeed
{
    public class ProductDataSeeder : IDataSeeder
    { 
        public void Seed(IDbContext context)
        {
            if (!context.Products.Any())
            {
                var products = new List<Product>()
                {
                    new Product("Apple Juice", "Apple Juice desc", ProductUnitType.Items, 0.33m),
                    new Product("Pear Juice", "Pear Juice desc", ProductUnitType.Items, 0.2m),
                    new Product("Raspberry Juice", "Raspberry Juice desc", ProductUnitType.Items, 0.5m),
                    new Product("Plum Juice", "Plum Juice desc", ProductUnitType.Items, 0.1m)
                };
                context.Products.AddRange(products);
            }

            context.SaveChanges();
        }
    }
}