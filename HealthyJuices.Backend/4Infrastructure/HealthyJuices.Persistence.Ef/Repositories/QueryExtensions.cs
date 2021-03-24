using System.Linq;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;

namespace HealthyJuices.Persistence.Ef.Repositories
{
    public static class QueryExtensions
    {
        public static IQueryable<TEntity> ById<TEntity>(this IQueryable<TEntity> query, string id) where TEntity : Entity
        {
            query = query.Where(m => m.Id == id);
            return query;
        }

        public static IQueryable<TEntity> TakeLast<TEntity>(this IQueryable<TEntity> query, int howMany) where TEntity : Entity
        {
            query = query.OrderByDescending(x => x.Id).Take(howMany);
            return query;
        }
    }
}