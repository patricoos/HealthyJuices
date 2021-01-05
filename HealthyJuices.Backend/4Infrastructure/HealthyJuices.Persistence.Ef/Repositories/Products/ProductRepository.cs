using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Products.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Products
{
    public class ProductRepository : PersistableRepository<Product>, IProductRepository
    {
        public ProductRepository(IDbContext context, ITimeProvider timeProvider) : base(context, timeProvider)
        {
        }

        public IProductQueryBuilder Query()
        {
            return new ProductQueryBuilder(AggregateRootDbSet.AsQueryable(), TimeProvider);
        }
    }
}