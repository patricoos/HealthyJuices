using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Orders.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Orders
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderQueryBuilder Query => new OrderQueryBuilder(AggregateRootDbSet.AsQueryable());
        public OrderRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetByUserIdAndDatesWithProductsAsync(string userId, DateTime? @from = null, DateTime? to = null)
        {
            var query = Query
                .IncludeProducts()
                .IsNotRemoved()
                .ByUser(userId);
            if (from.HasValue)
                query.AfterDateTime(from.Value);

            if (to.HasValue)
                query.BeforeDateTime(to.Value);

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetAllActiveWithUserAndDestinationCompanyAsync(DateTime? @from = null, DateTime? to = null)
        {
            var query = Query
                .IncludeUser()
                .IncludeDestinationCompany()
                .IsNotRemoved();

            if (from.HasValue)
                query.AfterDateTime(from.Value);

            if (to.HasValue)
                query.BeforeDateTime(to.Value);

            return await query.ToListAsync();
        }

        public Task<Order> GetByIdWithRelations(string id)
        {
            return Query
                .ById(id)
                .IncludeDestinationCompany()
                .IncludeProducts()
                .IncludeDestinationCompany()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetAllActiveWithRelationsAsync(DateTime? from = null, DateTime? to = null)
        {
            var query = Query
                .IncludeUser()
                .IncludeDestinationCompany()
                .IncludeProducts()
                .IsNotRemoved();

            if (from.HasValue)
                query.AfterDateTime(from.Value);

            if (to.HasValue)
                query.BeforeDateTime(to.Value);

            return await query.ToListAsync();
        }
    }
}