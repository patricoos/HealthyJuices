using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories
{
    public interface IWriteRepository<TAggregateRootEntity>
        where TAggregateRootEntity : IEntity, IAggregateRoot
    {
        IWriteRepository<TAggregateRootEntity> Insert(TAggregateRootEntity entity);
        IWriteRepository<TAggregateRootEntity> Insert(IEnumerable<TAggregateRootEntity> entities);
        IWriteRepository<TAggregateRootEntity> Update(TAggregateRootEntity entity);
        IWriteRepository<TAggregateRootEntity> Remove(TAggregateRootEntity entity);

        void SaveChanges();
        Task<int> SaveChangesAsync();
    }
}