using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace HealthyJuices.Persistence.Ef
{
    public interface IDbContext : IDisposable
    {
        ChangeTracker ChangeTracker { get; }
        DatabaseFacade Database { get; }

        EntityEntry Entry(object entity);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<IDbContextTransaction> BeginTransactionAsync();
        void DetachAllEntities();
    }
}