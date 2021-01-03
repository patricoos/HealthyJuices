using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Products.DataAccess
{
    public interface IProductRepository : IPersistableRepository<Product>, IQueryableRepository<Product, IProductQueryBuilder>
    {
    }
}