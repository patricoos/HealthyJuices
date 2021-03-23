using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Products.DataAccess
{
    public interface IProductRepository : IWriteRepository<Product>, IReadRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllActiveAsync();
        Task<IEnumerable<Product>> GetAllActiveByIdsAsync(string[] ids);
    }
}