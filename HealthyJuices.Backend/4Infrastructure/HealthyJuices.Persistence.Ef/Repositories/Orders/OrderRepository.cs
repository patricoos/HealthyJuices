using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Orders.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Orders
{
    public class OrderRepository : PersistableRepository<Order>, IOrderRepository
    {
        public OrderRepository(IDbContext context, ITimeProvider timeProvider) : base(context, timeProvider)
        {
        }

        public IOrderQueryBuilder Query()
        {
            return new OrderQueryBuilder(AggregateRootDbSet.AsQueryable(), TimeProvider);

        }
    }
}