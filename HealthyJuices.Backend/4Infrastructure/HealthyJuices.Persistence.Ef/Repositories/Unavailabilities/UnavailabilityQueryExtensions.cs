using System;
using System.Linq;
using HealthyJuices.Domain.Models.Unavailabilities;

namespace HealthyJuices.Persistence.Ef.Repositories.Unavailabilities
{
    public static class UnavailabilityQueryExtensions
    {
        public static IQueryable<TEntity> BeforeDateTime<TEntity>(this IQueryable<TEntity> query, DateTime date) where TEntity : Unavailability
        {
            query = query.Where(x => x.From <= date);
            return query;
        }

        public static IQueryable<TEntity> AfterDateTime<TEntity>(this IQueryable<TEntity> query, DateTime date) where TEntity : Unavailability
        {
            query = query.Where(x => x.To >= date);
            return query;
        }

        public static IQueryable<TEntity> BetweenDateTimes<TEntity>(this IQueryable<TEntity> query, DateTime @from, DateTime to) where TEntity : Unavailability
        {
            query.AfterDateTime(from);
            query.BeforeDateTime(to);
            return query;
        }
    }
}