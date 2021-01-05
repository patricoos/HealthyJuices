using System;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Orders.DataAccess
{
    public interface IOrderQueryBuilder : IQueryBuilder<Order, IOrderQueryBuilder>
    {
        IOrderQueryBuilder IsNotRemoved();
        IOrderQueryBuilder IncludeUser();
        IOrderQueryBuilder IncludeDestinationCompany();
        IOrderQueryBuilder IncludeProducts();
        IOrderQueryBuilder BeforeDateTime(DateTime date);
        IOrderQueryBuilder AfterDateTime(DateTime date);
        IOrderQueryBuilder BetweenDateTimes(DateTime from, DateTime to);
        IOrderQueryBuilder ByUser(long id);
    }
}