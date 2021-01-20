using System;
using System.Collections.Generic;
using System.Linq;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Persistence.Ef;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Api.Bootstrap.DataSeed
{
    public class OrderDataSeeder: IDataSeeder
    {
        public void Seed(IDbContext context)
        {
            if (!context.Orders.Any())
            {
                var orders = new List<Order>()
                {
                    new Order(context.Users.Include(x => x.Company).FirstOrDefault(), DateTime.Now.AddDays(1), context.Products.Where(x => x.IsActive).Skip(2).Take(2).Select(x => new OrderProduct{Product = x, Amount = 1}).ToArray()),
                    new Order(context.Users.Include(x => x.Company).FirstOrDefault(), DateTime.Now, context.Products.Where(x => x.IsActive).Take(3).Select(x => new OrderProduct{Product = x, Amount = 2}).ToArray()),
                    new Order(context.Users.Include(x => x.Company).FirstOrDefault(), DateTime.Now, context.Products.Where(x => x.IsActive).Skip(3).Select(x => new OrderProduct{Product = x, Amount = 5}).ToArray()),
                };
                context.Orders.AddRange(orders);
            }

            context.SaveChanges();
        }
    }
}