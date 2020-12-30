using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Orders.DataAccess
{
    public interface IOrderQueryBuilder : IQueryBuilder<Order, IOrderQueryBuilder>
    {
        
    }
}