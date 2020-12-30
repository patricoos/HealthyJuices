using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Orders.DataAccess
{
    public interface IOrderRepository : IPersistableRepository<Order>, IQueryableRepository<Order, IOrderQueryBuilder>
    {
        
    }
}