using System.Linq;
using HealthyJuices.Domain.Models.Products;

namespace HealthyJuices.Persistence.Ef.Repositories.Products
{
    public static class ProductQueryExtensions
    {
        public static IQueryable<TEntity> IsActive<TEntity>(this IQueryable<TEntity> query) where TEntity : Product
        {
            query = query.Where(x => x.IsActive == true);
            return query;
        }

        public static IQueryable<TEntity>  IsNotRemoved<TEntity>(this IQueryable<TEntity> query) where TEntity : Product
        {
            query = query.Where(x => x.IsRemoved == false);
            return query;
        }

        public static IQueryable<TEntity> ByIds<TEntity>(this IQueryable<TEntity> query, params string[] ids) where TEntity : Product
        {
            query = query.Where(x => ids.Contains(x.Id));
            return query;
        }
    }
}