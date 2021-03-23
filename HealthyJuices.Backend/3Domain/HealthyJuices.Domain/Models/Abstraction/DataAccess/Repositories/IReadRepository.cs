using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories
{
    public interface IReadRepository<TAggregateRootEntity>
        where TAggregateRootEntity : IEntity, IAggregateRoot
    {
        Task<IEnumerable<TAggregateRootEntity>> GetAllAsync();
        Task<TAggregateRootEntity> GetByIdAsync(string id, bool asNotTracking = true);
    }
}