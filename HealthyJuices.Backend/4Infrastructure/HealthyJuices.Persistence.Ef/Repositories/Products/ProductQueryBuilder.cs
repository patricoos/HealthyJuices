using System.Linq;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Products.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Products
{
    public class ProductQueryBuilder : QueryBuilder<Product, IProductQueryBuilder>, IProductQueryBuilder
    {
        public ProductQueryBuilder(IQueryable<Product> query, ITimeProvider timeProvider) : base(query, timeProvider)
        {
        }

        public IProductQueryBuilder IsActive()
        {
            Query = Query.Where(x => x.IsActive == true);
            return this;
        }

        public IProductQueryBuilder IsNotRemoved()
        {
            Query = Query.Where(x => x.IsRemoved == false);
            return this;
        }

        public IProductQueryBuilder ByIds(params string[] ids)
        {
            Query = Query.Where(x => ids.Contains(x.Id));
            return this;
        }
    }
}