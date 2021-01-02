using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories
{
    public abstract class PersistableRepository<TAggregateRootEntity> : IPersistableRepository<TAggregateRootEntity>
        where TAggregateRootEntity : Entity, IAggregateRoot
    {
        private readonly IDbContext _context;
        protected readonly ITimeProvider TimeProvider;

        protected DbSet<TAggregateRootEntity> AggregateRootDbSet => _context.Set<TAggregateRootEntity>();

        protected PersistableRepository(IDbContext context, ITimeProvider timeProvider)
        {
            _context = context;
            TimeProvider = timeProvider;
        }

        public virtual IPersistableRepository<TAggregateRootEntity> Insert(TAggregateRootEntity entity)
        {
            AggregateRootDbSet.Add(entity);
            return this;
        }

        public virtual IPersistableRepository<TAggregateRootEntity> Insert(IEnumerable<TAggregateRootEntity> entities)
        {
            AggregateRootDbSet.AddRange(entities);
            return this;
        }

        public virtual IPersistableRepository<TAggregateRootEntity> Update(TAggregateRootEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return this;
        }

        public virtual IPersistableRepository<TAggregateRootEntity> Remove(TAggregateRootEntity entity)
        {
            AggregateRootDbSet.Remove(entity);
            return this;
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void ClearAllChanges()
        {
            _context.DetachAllEntities();
        }
    }
}