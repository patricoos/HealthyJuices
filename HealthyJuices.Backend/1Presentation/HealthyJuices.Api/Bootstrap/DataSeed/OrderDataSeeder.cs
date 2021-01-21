﻿using System;
using System.Collections.Generic;
using System.Linq;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Products;
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
                    new Order(context.Users.Include(x => x.Company).FirstOrDefault(), DateTime.Now.AddDays(1), context.Products.Where(x => x.IsActive).Skip(2).Take(2).ToDictionary<Product, Product, decimal>(x => x, x =>  1)),
                    new Order(context.Users.Include(x => x.Company).FirstOrDefault(), DateTime.Now, context.Products.Where(x => x.IsActive).Take(3).ToDictionary<Product, Product, decimal>(x => x, x =>  2)),
                    new Order(context.Users.Include(x => x.Company).FirstOrDefault(), DateTime.Now, context.Products.Where(x => x.IsActive).Skip(3).ToDictionary<Product, Product, decimal>(x => x, x =>  5)),
                };
                context.Orders.AddRange(orders);
            }

            context.SaveChanges();
        }
    }
}