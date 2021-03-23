using System.Linq;
using HealthyJuices.Domain.Models.Products;

namespace HealthyJuices.Persistence.Ef.Repositories.Products
{
    public class ProductQueryBuilder : QueryBuilder<Product, ProductQueryBuilder>
    {
        public ProductQueryBuilder(IQueryable<Product> query) : base(query)
        {
        }

        public ProductQueryBuilder IsActive()
        {
            Query = Query.Where(x => x.IsActive == true);
            return this;
        }

        public ProductQueryBuilder IsNotRemoved()
        {
            Query = Query.Where(x => x.IsRemoved == false);
            return this;
        }

        public ProductQueryBuilder ByIds(params string[] ids)
        {
            Query = Query.Where(x => ids.Contains(x.Id));
            return this;
        }
    }
}