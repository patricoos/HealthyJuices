using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Orders.DataAccess
{
    public interface IOrderRepository : IWriteRepository<Order>, IReadRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAndDatesWithProductsAsync(string userId, DateTime? from = null, DateTime? to = null);
        Task<IEnumerable<Order>> GetAllActiveWithUserAndDestinationCompanyAsync(DateTime? @from = null, DateTime? to = null);
        Task<IEnumerable<Order>> GetAllActiveWithRelationsAsync(DateTime? @from = null, DateTime? to = null);
        Task<Order> GetByIdWithRelations(string id);
    }
}