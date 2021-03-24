using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using MongoDB.Driver;

namespace HealthyJuices.Persistence.MongoDb.Repositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IMongoDbClient client) : base(client.Companies)
        {
        }

        public async Task<IEnumerable<Company>> GetAllActiveAsync()
        {
            return await Collection.Find(x => !x.IsRemoved).ToListAsync();
        }
    }
}