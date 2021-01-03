using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Companies.DataAccess
{
    public interface ICompanyRepository : IPersistableRepository<Company>, IQueryableRepository<Company, ICompanyQueryBuilder>
    {
    }
}