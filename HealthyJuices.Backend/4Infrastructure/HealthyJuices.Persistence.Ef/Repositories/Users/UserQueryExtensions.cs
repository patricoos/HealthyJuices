using System.Linq;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories.Users
{
    public static class UserQueryExtensions
    {
        public static IQueryable<TEntity> ByEmail<TEntity>(this IQueryable<TEntity> query, string email) where TEntity : User
        {
            query = query.Where(x => x.Email == email);
            return query;
        }

        public static IQueryable<TEntity> ByUserRole<TEntity>(this IQueryable<TEntity> query, UserRole role) where TEntity : User
        {
            query = query.Where(x => x.Roles.HasFlag(role));
            return query;
        }

        public static IQueryable<TEntity> IsActive<TEntity>(this IQueryable<TEntity> query) where TEntity : User
        {
            query = query.Where(x => x.IsActive == true);
            return query;
        }

        public static IQueryable<TEntity> IncludeCompany<TEntity>(this IQueryable<TEntity> query) where TEntity : User
        {
            query = query.Include(x => x.Company);
            return query;
        }

        public static IQueryable<TEntity> IsNotRemoved<TEntity>(this IQueryable<TEntity> query) where TEntity : User
        {
            query = query.Where(x => x.IsRemoved == false);
            return query;
        }
    }
}