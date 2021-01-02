using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories
{
    public abstract class QueryBuilder<TEntity, TQueryBuilder> : IQueryBuilder<TEntity, TQueryBuilder>
           where TEntity : Entity
           where TQueryBuilder : class
    {
        protected readonly ITimeProvider TimeProvider;
        protected List<string> IgnoredProperties;

        protected QueryBuilder(IQueryable<TEntity> query, ITimeProvider timeProvider)
        {
            Query = query;
            TimeProvider = timeProvider;
            IgnoredProperties = new List<string>();
        }

        protected IQueryable<TEntity> Query { get; set; }

        public TQueryBuilder Exclude<TProperty>(Expression<Func<TEntity, TProperty>> selector)
        {
            IgnoredProperties.Add(GetPropertyName(selector));

            return this as TQueryBuilder;
        }

        public TQueryBuilder ById(long id)
        {
            Query = Query.Where(m => m.Id == id);
            return this as TQueryBuilder;
        }

        public TQueryBuilder Include<T>(Expression<Func<TEntity, T>> prop)
        {
            Query = Query.Include(prop);
            return this as TQueryBuilder;
        }

        public TQueryBuilder Include(string prop)
        {
            Query = Query.Include(prop);
            return this as TQueryBuilder;
        }

        public TQueryBuilder OrderBy<T>(Expression<Func<TEntity, T>> prop)
        {
            Query = Query.OrderBy(prop);
            return this as TQueryBuilder;
        }

        public TQueryBuilder OrderByDescending<T>(Expression<Func<TEntity, T>> prop)
        {
            Query = Query.OrderByDescending(prop);
            return this as TQueryBuilder;
        }

        public TQueryBuilder Take(int count)
        {
            Query = Query.Take(count);
            return this as TQueryBuilder;
        }

        public TQueryBuilder TakeLast(int howMany)
        {
            Query = Query.OrderByDescending(x => x.Id).Take(howMany);
            return this as TQueryBuilder;
        }

        public TQueryBuilder After(long id)
        {
            Query = Query.Where(m => m.Id >= id);
            return this as TQueryBuilder;
        }

        public TQueryBuilder Before(long id)
        {
            Query = Query.Where(m => m.Id <= id);
            return this as TQueryBuilder;
        }

        public TQueryBuilder AsNoTracking()
        {
            Query = Query.AsNoTracking();
            return this as TQueryBuilder;
        }

        public TEntity FirstOrDefault()
        {
            return Query.FirstOrDefault();
        }

        public async Task<TEntity> FirstOrDefaultAsync()
        {
            return await Query.FirstOrDefaultAsync();
        }

        public TQueryBuilder Select(Expression<Func<TEntity, TEntity>> func)
        {
            Query = Query.Select(func);

            return this as TQueryBuilder;
        }

        public async Task<List<TProjectionObject>> GetProjectionAsync<TProjectionObject>(Expression<Func<TEntity, TProjectionObject>> func)
            where TProjectionObject : new()
        {
            var result = await Query
                .AsNoTracking()
                .Select(func)
                .ToListAsync();

            return result;
        }

        public async Task<TProjectionObject> GetFirstProjectionAsync<TProjectionObject>(Expression<Func<TEntity, TProjectionObject>> func)
            where TProjectionObject : new()
        {
            var result = await Query
                .AsNoTracking()
                .Select(func)
                .FirstOrDefaultAsync();

            return result;
        }

        public TQueryBuilder Distinct()
        {
            Query = Query.Distinct();
            return this as TQueryBuilder;
        }

        public TQueryBuilder InjectPredicate(Expression<Func<TEntity, bool>> predicate)
        {
            this.Query = Query.Where(predicate);
            return this as TQueryBuilder;
        }

        private static string GetPropertyName<TProperty>(Expression<Func<TEntity, TProperty>> selector)
        {
            try
            {
                return ((PropertyInfo)((MemberExpression)selector.Body).Member).Name;
            }
            catch
            {
                return string.Empty;
            }
        }

        protected virtual TEntity ToEntity(TEntity entity)
        {
            var properties = typeof(TEntity).GetProperties().Where(x => x.CanWrite && x.GetSetMethod() != null);
            var instance = Activator.CreateInstance<TEntity>();

            foreach (var prop in properties)
            {
                if (IgnoredProperties.Contains(prop.Name)) continue;
                instance.GetType().GetProperty(prop.Name).SetValue(instance, entity.GetType().GetProperty(prop.Name).GetValue(entity));
            }

            return instance;
        }

        public List<TEntity> ToList()
        {
            return Query.ToList();
        }

        public async Task<List<TEntity>> ToListAsync()
        {
            if (IgnoredProperties.Any())
            {
                Query = Query.Select(x => ToEntity(x));
                return await Query.ToListAsync();
            }
            else
                return await Query.ToListAsync();
        }

        public async Task<List<TEntity>> ToNonTrackingListAsync()
        {
            if (IgnoredProperties.Any())
                return await Query.Select(x => ToEntity(x)).AsNoTracking().ToListAsync();
            else
                return await Query.AsNoTracking().ToListAsync();
        }

        public int Count()
        {
            return Query.Count();
        }

        public async Task<int> CountAsync()
        {
            return await Query.CountAsync();
        }

        public bool Any()
        {
            return Query.Any();
        }

        public async Task<bool> AnyAsync()
        {
            return await Query.AnyAsync();
        }

        public TEntity SingleOrDefault()
        {
            return Query.SingleOrDefault();
        }

        public TEntity Single()
        {
            try
            {
                return Query.Single();
            }
            catch (Exception)
            {
                throw new Exception($"Signle() failed with {typeof(TEntity).Name}");
            }
        }

        public async Task<TEntity> SingleOrDefaultAsync()
        {
            return await Query.SingleOrDefaultAsync();
        }

        public async Task<TEntity> SingleAsync()
        {
            try
            {
                return await Query.SingleAsync();
            }
            catch (Exception)
            {
                throw new Exception($"Signle() failed with {typeof(TEntity).Name}. {Query.Expression}");
            }
        }

        public async Task<TEntity> FirstAsync()
        {
            try
            {
                return await Query.FirstAsync();
            }
            catch (Exception)
            {
                throw new Exception($"First() failed with {typeof(TEntity).Name}. {Query.Expression}");
            }
        }

    }
}