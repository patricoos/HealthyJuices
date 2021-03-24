using System;
using System.Linq;
using HealthyJuices.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories.Orders
{
    public static class OrderQueryExtensions
    {
        public static IQueryable<TEntity> IsNotRemoved<TEntity>(this IQueryable<TEntity> query) where TEntity : Order
        {
            query = query.Where(x => x.IsRemoved == false);
            return query;
        }

        public static IQueryable<TEntity> IncludeUser<TEntity>(this IQueryable<TEntity> query) where TEntity : Order
        {
            query = query.Include(x => x.User);
            return query;
        }

        public static IQueryable<TEntity> IncludeDestinationCompany<TEntity>(this IQueryable<TEntity> query) where TEntity : Order
        {
            query = query.Include(x => x.DestinationCompany);
            return query;
        }

        public static IQueryable<TEntity> IncludeProducts<TEntity>(this IQueryable<TEntity> query) where TEntity : Order
        {
            query = query.Include(x => x.OrderProducts).ThenInclude(x => x.Product);
            return query;
        }

        public static IQueryable<TEntity> BeforeDateTime<TEntity>(this IQueryable<TEntity> query,DateTime date) where TEntity : Order
        {
            query = query.Where(x => x.DeliveryDate <= date);
            return query;
        }

        public static IQueryable<TEntity> AfterDateTime<TEntity>(this IQueryable<TEntity> query, DateTime date) where TEntity : Order
        {
            query = query.Where(x => x.DeliveryDate >= date);
            return query;
        }

        public static IQueryable<TEntity> BetweenDateTimes<TEntity>(this IQueryable<TEntity> query, DateTime @from, DateTime to) where TEntity : Order
        {
            query.AfterDateTime(from);
            query.BeforeDateTime(to);

            return query;
        }

        public static IQueryable<TEntity> ByUser<TEntity>(this IQueryable<TEntity> query, string id) where TEntity : Order
        {
            query = query.Where(x => x.UserId == id);
            return query;
        }
    }
}