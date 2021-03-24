using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;
using MongoDB.Driver;

namespace HealthyJuices.Persistence.MongoDb.Repositories
{
    public abstract class BaseRepository<TAggregateRootEntity> : IWriteRepository<TAggregateRootEntity>, IReadRepository<TAggregateRootEntity>
        where TAggregateRootEntity : Entity, IAggregateRoot
    {
        protected readonly IMongoCollection<TAggregateRootEntity> Collection;

        protected BaseRepository(IMongoCollection<TAggregateRootEntity> collection)
        {
            Collection = collection;
        }

        public virtual IWriteRepository<TAggregateRootEntity> Insert(TAggregateRootEntity entity)
        {
            Collection.InsertOne(entity);
            return this;
        }

        public virtual IWriteRepository<TAggregateRootEntity> Insert(IEnumerable<TAggregateRootEntity> entities)
        {
            Collection.InsertMany(entities);
            return this;
        }

        public virtual IWriteRepository<TAggregateRootEntity> Update(TAggregateRootEntity entity)
        {
            Collection.ReplaceOne(e => e.Id == entity.Id, entity);
            return this;
        }

        public virtual IWriteRepository<TAggregateRootEntity> Remove(TAggregateRootEntity entity)
        {
            Collection.DeleteOne(x => x.Id == entity.Id);
            return this;
        }

        public virtual void SaveChanges()
        {
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return Task.FromResult(-1);
        }

        public virtual async Task<IEnumerable<TAggregateRootEntity>> GetAllAsync(bool asNotTrackong = true)
        {
           return await Collection.Find(book => true).ToListAsync();
        }

        public virtual async Task<TAggregateRootEntity> GetByIdAsync(string id, bool asNotTrackong = true)
        {
            return await Collection.Find(book => book.Id == id).FirstOrDefaultAsync();
        }
    }
}