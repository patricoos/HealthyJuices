using System;
using System.Linq;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories.Orders
{
    public class OrderQueryBuilder : QueryBuilder<Order, OrderQueryBuilder>
    {
        public OrderQueryBuilder(IQueryable<Order> query) : base(query)
        {
        }

        public OrderQueryBuilder IsNotRemoved()
        {
            Query = Query.Where(x => x.IsRemoved == false);
            return this;
        }

        public OrderQueryBuilder IncludeUser()
        {
            Query = Query.Include(x => x.User);
            return this;
        }

        public OrderQueryBuilder IncludeDestinationCompany()
        {
            Query = Query.Include(x => x.DestinationCompany);
            return this;
        }

        public OrderQueryBuilder IncludeProducts()
        {
            Query = Query.Include(x => x.OrderProducts).ThenInclude(x => x.Product);
            return this;
        }

        public OrderQueryBuilder BeforeDateTime(DateTime date)
        {
            Query = Query.Where(x => x.DeliveryDate <= date);
            return this;
        }

        public OrderQueryBuilder AfterDateTime(DateTime date)
        {
            Query = Query.Where(x => x.DeliveryDate >= date);
            return this;
        }

        public OrderQueryBuilder BetweenDateTimes(DateTime @from, DateTime to)
        {
            AfterDateTime(from);
            BeforeDateTime(to);

            return this;
        }

        public OrderQueryBuilder ByUser(string id)
        {
            Query = Query.Where(x => x.UserId == id);
            return this;
        }
    }
}