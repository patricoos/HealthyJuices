using System.Linq;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Orders;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories.Orders
{
    public class OrderQueryBuilder : QueryBuilder<Order, IOrderQueryBuilder>, IOrderQueryBuilder
    {
        public OrderQueryBuilder(IQueryable<Order> query, ITimeProvider timeProvider) : base(query, timeProvider)
        {
        }

        public IOrderQueryBuilder IsNotRemoved()
        {
            Query = Query.Where(x => x.IsRemoved == false);
            return this;
        }

        public IOrderQueryBuilder IncludeUser()
        {
            Query = Query.Include(x => x.User);
            return this;
        }

        public IOrderQueryBuilder IncludeDestinationCompany()
        {
            Query = Query.Include(x => x.DestinationCompany);
            return this;
        }
    }
}