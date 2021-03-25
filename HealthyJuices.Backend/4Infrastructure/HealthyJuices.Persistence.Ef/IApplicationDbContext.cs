using System;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Products;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace HealthyJuices.Persistence.Ef
{
    public interface IApplicationDbContext : IDisposable
    {
        #region - DbSets 

        DbSet<Log> Logs { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Unavailability> Unavailabilities { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<OrderItem> OrderProducts { get; set; }

        #endregion

        ChangeTracker ChangeTracker { get; }
        DatabaseFacade Database { get; }

        EntityEntry Entry(object entity);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}