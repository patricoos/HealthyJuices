using System;
using System.Linq;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Persistence.Ef.Repositories.Logs
{
    public static class LogQueryExtensions
    {
        public static IQueryable<TEntity> BeforeDateTime<TEntity>(this IQueryable<TEntity> query, DateTime date) where TEntity : Log
        {
            query = query.Where(x => x.Date <= date);
            return query;
        }

        public static IQueryable<TEntity> AfterDateTime<TEntity>(this IQueryable<TEntity> query, DateTime date) where TEntity : Log
        {
            query = query.Where(x => x.Date >= date);
            return query;
        }

        public static IQueryable<TEntity> BetweenDateTimes<TEntity>(this IQueryable<TEntity> query, DateTime from, DateTime to) where TEntity : Log
        {
            query.AfterDateTime(from);
            query.BeforeDateTime(to);

            return query;
        }

        public static IQueryable<TEntity> ByType<TEntity>(this IQueryable<TEntity> query, LogType type) where TEntity : Log
        {
            query = query.Where(x => x.Type == type);
            return query;
        }
    }
}