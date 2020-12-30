using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories
{
    public interface IPersistableRepository<TAggregateRootEntity>
        where TAggregateRootEntity : IEntity, IAggregateRoot
    {
        void ClearAllChanges();

        IPersistableRepository<TAggregateRootEntity> Insert(TAggregateRootEntity entity);
        IPersistableRepository<TAggregateRootEntity> Insert(IEnumerable<TAggregateRootEntity> entities);
        IPersistableRepository<TAggregateRootEntity> Update(TAggregateRootEntity entity);
        IPersistableRepository<TAggregateRootEntity> Remove(TAggregateRootEntity entity);

        void SaveChanges();
        Task<int> SaveChangesAsync();
    }
}