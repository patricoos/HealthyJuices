using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Products.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Products
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductQueryBuilder Query => new ProductQueryBuilder(AggregateRootDbSet.AsQueryable());
        
        public ProductRepository(IDbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<Product>> GetAllActiveAsync()
        {
            return await Query.IsActive().IsNotRemoved().ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllActiveByIdsAsync(string[] ids)
        {
            return await Query.IsActive().IsNotRemoved().ByIds(ids).ToListAsync();
        }
    }
}