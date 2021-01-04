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
                    new Order(context.Users.Include(x => x.Company).FirstOrDefault(), DateTime.Now, context.Products.Take(2).ToList()),
                    new Order(context.Users.Include(x => x.Company).FirstOrDefault(), DateTime.Now.AddDays(1), context.Products.Skip(1).Take(2).ToList())
                };
                context.Orders.AddRange(orders);
            }

            context.SaveChanges();
        }
    }
}