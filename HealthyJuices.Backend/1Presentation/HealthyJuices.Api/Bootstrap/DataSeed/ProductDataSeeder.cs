using System;
using System.Linq;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthyJuices.Api.Bootstrap.DataSeed
{
    public class ProductDataSeeder : IDataSeeder
    { 
        public void Seed(IDbContext context)
        {
            if (!context.Products.Any())
            {
                var product = new Product("test Juice", "test desc", ProductUnitType.Items, 0.33m);
                context.Products.Add(product);
            }

            context.SaveChanges();
        }
    }
}