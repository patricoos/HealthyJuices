using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories
{
    public interface IQueryBuilder<TEntity, out TQueryBuilder> where TEntity : Entity where TQueryBuilder : class
    {
        TQueryBuilder ById(string id);
        TQueryBuilder Include<T>(Expression<Func<TEntity, T>> prop);
        TQueryBuilder Include(string prop);
        TQueryBuilder OrderBy<T>(Expression<Func<TEntity, T>> prop);
        TQueryBuilder OrderByDescending<T>(Expression<Func<TEntity, T>> prop);
        TQueryBuilder Take(int count);
        TQueryBuilder TakeLast(int howMany);
        TQueryBuilder AsNoTracking();
        TQueryBuilder Exclude<TProperty>(Expression<Func<TEntity, TProperty>> selector);
        TQueryBuilder Select(Expression<Func<TEntity, TEntity>> func);
        TQueryBuilder Distinct();

        TQueryBuilder InjectPredicate(Expression<Func<TEntity, bool>> predicate);

        Task<List<TProjectionObject>> GetProjectionAsync<TProjectionObject>(
            Expression<Func<TEntity, TProjectionObject>> func)
            where TProjectionObject : new();

        Task<TProjectionObject> GetFirstProjectionAsync<TProjectionObject>(
            Expression<Func<TEntity, TProjectionObject>> func)
            where TProjectionObject : new();

        TEntity SingleOrDefault();
        Task<TEntity> SingleOrDefaultAsync();
        TEntity Single();
        Task<TEntity> SingleAsync();
        TEntity FirstOrDefault();
        Task<TEntity> FirstOrDefaultAsync();
        List<TEntity> ToList();
        Task<List<TEntity>> ToListAsync();
        Task<List<TEntity>> ToNonTrackingListAsync();
        Task<TEntity> FirstAsync();
        int Count();
        Task<int> CountAsync();
        bool Any();
        Task<bool> AnyAsync();
    }
}