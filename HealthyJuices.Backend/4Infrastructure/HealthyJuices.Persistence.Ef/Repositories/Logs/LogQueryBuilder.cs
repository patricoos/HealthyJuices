using System;
using System.Linq;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Persistence.Ef.Repositories.Logs
{
    public class LogQueryBuilder : QueryBuilder<Log, LogQueryBuilder>
    {
        public LogQueryBuilder(IQueryable<Log> query) : base(query)
        {
        }

        public LogQueryBuilder BeforeDateTime(DateTime date)
        {
            Query = Query.Where(x => x.Date <= date);
            return this;
        }

        public LogQueryBuilder AfterDateTime(DateTime date)
        {
            Query = Query.Where(x => x.Date >= date);
            return this;
        }

        public LogQueryBuilder BetweenDateTimes(DateTime from, DateTime to)
        {
            AfterDateTime(from);
            BeforeDateTime(to);

            return this;
        }

        public LogQueryBuilder ByType(LogType type)
        {
            Query = Query.Where(x => x.Type == type);
            return this;
        }
    }
}