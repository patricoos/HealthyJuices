using System;
using System.Linq;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Logs.DataAccess;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Persistence.Ef.Repositories.Logs
{
    public class LogQueryBuilder : QueryBuilder<Log, ILogQueryBuilder>, ILogQueryBuilder
    {
        public LogQueryBuilder(IQueryable<Log> query, ITimeProvider timeProvider) : base(query, timeProvider)
        {
        }

        public ILogQueryBuilder BeforeDateTime(DateTime date)
        {
            Query = Query.Where(x => x.Date <= date);
            return this;
        }

        public ILogQueryBuilder AfterDateTime(DateTime date)
        {
            Query = Query.Where(x => x.Date >= date);
            return this;
        }

        public ILogQueryBuilder BetweenDateTimes(DateTime from, DateTime to)
        {
            AfterDateTime(from);
            BeforeDateTime(to);

            return this;
        }

        public ILogQueryBuilder ByType(LogType type)
        {
            Query = Query.Where(x => x.Type == type);
            return this;
        }
    }
}