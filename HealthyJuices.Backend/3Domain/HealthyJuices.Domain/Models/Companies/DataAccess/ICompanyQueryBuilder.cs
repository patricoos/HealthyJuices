using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Companies.DataAccess
{
    public interface ICompanyQueryBuilder : IQueryBuilder<Company, ICompanyQueryBuilder>
    {
        ICompanyQueryBuilder IsNotRemoved();
    }
}