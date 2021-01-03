using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Companies.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Companies
{
    public class CompanyRepository : PersistableRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IDbContext context, ITimeProvider timeProvider) : base(context, timeProvider)
        {
        }

        public ICompanyQueryBuilder Query()
        {
            return new CompanyQueryBuilder(AggregateRootDbSet.AsQueryable(), TimeProvider);
        }
    }
}