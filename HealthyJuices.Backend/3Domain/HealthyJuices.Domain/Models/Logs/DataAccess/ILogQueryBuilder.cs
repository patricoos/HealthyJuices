using System;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Logs.DataAccess
{
    public interface ILogQueryBuilder : IQueryBuilder<Log, ILogQueryBuilder>
    {
        ILogQueryBuilder BeforeDateTime(DateTime date);
        ILogQueryBuilder AfterDateTime(DateTime date);
        ILogQueryBuilder BetweenDateTimes(DateTime from, DateTime to);
        ILogQueryBuilder ByType(LogType type);

    }
}