using System.Linq;
using HealthyJuices.Domain.Models.Companies;

namespace HealthyJuices.Persistence.Ef.Repositories.Companies
{
    public class CompanyQueryBuilder : QueryBuilder<Company, CompanyQueryBuilder>
    {
        public CompanyQueryBuilder(IQueryable<Company> query) : base(query)
        {
        }

        public CompanyQueryBuilder IsNotRemoved()
        {
            Query = Query.Where(x => x.IsRemoved == false);
            return this;
        }
    }
}