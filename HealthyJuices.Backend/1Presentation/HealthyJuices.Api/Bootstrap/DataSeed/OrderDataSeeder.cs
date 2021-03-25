using System;
using System.Collections.Generic;
using System.Linq;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Api.Bootstrap.DataSeed
{
    public class OrderDataSeeder: IDataSeeder
    {
        public void Seed(IApplicationDbContext context)
        {
            if (!context.Orders.Any())
            {
                var user = context.Users.Include(x => x.Company).FirstOrDefault(x => x.Roles.HasFlag(UserRole.Customer));
                var orders = new List<Order>
                {
                    new Order(user, DateTime.Now.AddDays(2), context.Products.Where(x => x.IsActive).Skip(2).Take(2).Select(x => new KeyValuePair<Product, decimal>(x, 1))),
                    new Order(user, DateTime.Now.AddDays(1), context.Products.Where(x => x.IsActive).Take(3).Select(x => new KeyValuePair<Product, decimal>(x,  2))),
                    new Order(user, DateTime.Now.AddDays(1), context.Products.Where(x => x.IsActive).Skip(3).Select(x => new KeyValuePair<Product, decimal>(x, 5)))
                };
                context.Orders.AddRange(orders);
            }

            context.SaveChanges();
        }
    }
}