using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Companies.DataAccess
{
    public interface ICompanyRepository : IWriteRepository<Company>, IReadRepository<Company>
    {
        Task<IEnumerable<Company>> GetAllActiveAsync();
    }
}