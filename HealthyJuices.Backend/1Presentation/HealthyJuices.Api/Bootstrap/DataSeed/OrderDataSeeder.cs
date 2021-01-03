using System;
using System.Collections.Generic;
using System.Linq;
using HealthyJuices.Common;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Api.Bootstrap.DataSeed
{
    public class OrderDataSeeder: IDataSeeder
    {
        public void Seed(IDbContext context)
        {
            if (!context.Orders.Any())
            {
                var order = new Order(context.Users.FirstOrDefault(), DateTime.Now, new List<Product>(){context.Products.FirstOrDefault()});
                context.Orders.Add(order);
            }

            context.SaveChanges();
        }
    }
}