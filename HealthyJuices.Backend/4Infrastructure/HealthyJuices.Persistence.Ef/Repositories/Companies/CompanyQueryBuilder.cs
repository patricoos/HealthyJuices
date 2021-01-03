using System.Linq;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Companies.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Companies
{
    public class CompanyQueryBuilder : QueryBuilder<Company, ICompanyQueryBuilder>, ICompanyQueryBuilder
    {
        public CompanyQueryBuilder(IQueryable<Company> query, ITimeProvider timeProvider) : base(query, timeProvider)
        {
        }

        public ICompanyQueryBuilder IsNotRemoved()
        {
            Query = Query.Where(x => x.IsRemoved == false);
            return this;
        }
    }
}