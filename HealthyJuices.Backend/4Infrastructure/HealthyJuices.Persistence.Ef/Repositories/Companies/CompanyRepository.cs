using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories.Companies
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Company>> GetAllActiveAsync()
        {
            return await Query.IsNotRemoved().ToListAsync();
        }
    }
}
