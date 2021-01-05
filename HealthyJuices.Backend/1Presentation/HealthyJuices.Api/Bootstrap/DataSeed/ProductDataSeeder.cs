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
                    new Product("Apple Juice", "Apple Juice desc", ProductUnitType.Liters, 0.33m, true),
                    new Product("Pear Juice", "Pear Juice desc", ProductUnitType.Liters,  0.2m, true),
                    new Product("Raspberry Juice", "Raspberry Juice desc", ProductUnitType.Liters,  0.5m, true),
                    new Product("Plum Juice", "Plum Juice desc", ProductUnitType.Liters, 0.1m, false),
                    new Product("Pineapple Juice", "Plum Juice desc", ProductUnitType.Liters, 0.4m, true)
                };
                context.Products.AddRange(products);
            }

            context.SaveChanges();
        }
    }
}