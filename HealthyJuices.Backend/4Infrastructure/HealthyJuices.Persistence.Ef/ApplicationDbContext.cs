using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Persistence.Ef.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;

namespace HealthyJuices.Persistence.Ef
{
    public class ApplicationApplicationDbContext : DbContext, IApplicationDbContext
    {
        #region - DbSets 
        public DbSet<Log> Logs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Unavailability> Unavailabilities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderProducts { get; set; }
        #endregion

        protected readonly CancellationToken CancellationToken;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IDateTimeProvider _timeProvider;

        public ApplicationApplicationDbContext(DbContextOptions<ApplicationApplicationDbContext> options, ICurrentUserProvider currentUserProvider, IDateTimeProvider timeProvider) : this(options, currentUserProvider, timeProvider, CancellationToken.None)
        {
        }

        public ApplicationApplicationDbContext(DbContextOptions<ApplicationApplicationDbContext> options, ICurrentUserProvider currentUserProvider, IDateTimeProvider timeProvider, CancellationToken cancellationToken) : base(options)
        {
            CancellationToken = cancellationToken;
            _currentUserProvider = currentUserProvider;
            _timeProvider = timeProvider;

            if (options.FindExtension<InMemoryOptionsExtension>() == null)
                base.Database.SetCommandTimeout(300);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<IEntity> entry in ChangeTracker.Entries<IEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetPrivateProperty(entry.Entity, nameof(IEntity.Created), (DateTimeOffset)_timeProvider.UtcNow);
                        SetPrivateProperty(entry.Entity, nameof(IEntity.CreatedBy), _currentUserProvider.UserId);
                        break;

                    case EntityState.Modified:
                        SetPrivateProperty(entry.Entity, nameof(IEntity.LastModified), (DateTimeOffset)_timeProvider.UtcNow);
                        SetPrivateProperty(entry.Entity, nameof(IEntity.LastModifiedBy), _currentUserProvider.UserId);
                        break;
                }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        private static void SetPrivateProperty(object instance, string propertyName, object newValue)
        {
            Type type = instance.GetType();
            PropertyInfo prop = type.BaseType.GetProperty(propertyName);
            prop.SetValue(instance, newValue, null);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyEfConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}