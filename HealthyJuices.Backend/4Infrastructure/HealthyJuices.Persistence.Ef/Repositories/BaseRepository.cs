using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories
{
    public abstract class BaseRepository<TAggregateRootEntity> : IWriteRepository<TAggregateRootEntity>, IReadRepository<TAggregateRootEntity>
        where TAggregateRootEntity : Entity, IAggregateRoot
    {
        private readonly IDbContext _context;
        protected DbSet<TAggregateRootEntity> AggregateRootDbSet => _context.Set<TAggregateRootEntity>();
        internal IQueryable<TAggregateRootEntity> Query => AggregateRootDbSet.AsQueryable();


        protected BaseRepository(IDbContext context)
        {
            _context = context;
        }

        public virtual IWriteRepository<TAggregateRootEntity> Insert(TAggregateRootEntity entity)
        {
            AggregateRootDbSet.Add(entity);
            return this;
        }

        public virtual IWriteRepository<TAggregateRootEntity> Insert(IEnumerable<TAggregateRootEntity> entities)
        {
            AggregateRootDbSet.AddRange(entities);
            return this;
        }

        public virtual IWriteRepository<TAggregateRootEntity> Update(TAggregateRootEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return this;
        }

        public virtual IWriteRepository<TAggregateRootEntity> Remove(TAggregateRootEntity entity)
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

        public virtual async Task<IEnumerable<TAggregateRootEntity>> GetAllAsync(bool asNotTrackong = true)
        {
            var result = await Query.ToListAsync();
            return result;
        }

        public virtual async Task<TAggregateRootEntity> GetByIdAsync(string id, bool asNotTrackong = true)
        {
            var query = Query;
            if (asNotTrackong)
                query = query.AsNoTracking();

            return await query.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}