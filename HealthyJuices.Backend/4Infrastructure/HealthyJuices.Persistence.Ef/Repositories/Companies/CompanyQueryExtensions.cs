using System.Linq;
using HealthyJuices.Domain.Models.Companies;

namespace HealthyJuices.Persistence.Ef.Repositories.Companies
{
    public static class CompanyQueryExtensions 
    {
        public static IQueryable<TEntity> IsNotRemoved<TEntity>(this IQueryable<TEntity> query) where TEntity : Company
        {
            query = query.Where(x => x.IsRemoved == false);
            return query;
        }
    }
}