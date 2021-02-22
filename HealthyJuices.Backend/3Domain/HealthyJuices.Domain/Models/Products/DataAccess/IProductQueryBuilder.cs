using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Products.DataAccess
{
    public interface IProductQueryBuilder : IQueryBuilder<Product, IProductQueryBuilder>
    {
        IProductQueryBuilder IsActive();
        IProductQueryBuilder IsNotRemoved();
        IProductQueryBuilder ByIds(params string[] ids);
    }
}