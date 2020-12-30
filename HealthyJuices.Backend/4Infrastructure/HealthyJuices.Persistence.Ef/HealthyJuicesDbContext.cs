using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Unavailabilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace HealthyJuices.Persistence.Ef
{
    public class HealthyJuicesDbContext : DbContext, IDbContext
    {
        #region - DbSets 

        public DbSet<Log> Logs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Unavailability> Unavailabilities { get; set; }
        public DbSet<Company> Companies { get; set; }

        #endregion


        protected readonly CancellationToken CancellationToken;

        public HealthyJuicesDbContext(DbContextOptions<HealthyJuicesDbContext> options) : this(options, CancellationToken.None)
        {
        }

        public HealthyJuicesDbContext(DbContextOptions<HealthyJuicesDbContext> options, CancellationToken cancellationToken) : base(options)
        {
            CancellationToken = cancellationToken;

            if (options.FindExtension<InMemoryOptionsExtension>() == null)
            {
                base.Database.SetCommandTimeout(300);
            }
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return base.Database.BeginTransactionAsync(CancellationToken);
        }
    }
}