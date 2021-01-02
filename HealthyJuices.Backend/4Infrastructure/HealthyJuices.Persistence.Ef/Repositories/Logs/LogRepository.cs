using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Logs.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Logs
{
    public class LogRepository : PersistableRepository<Log>, ILogRepository
    {
        public LogRepository(IDbContext context, ITimeProvider timeProvider) : base(context, timeProvider)
        {
        }

        public ILogQueryBuilder Query()
        {
            return new LogQueryBuilder(AggregateRootDbSet.AsQueryable(), TimeProvider);
        }
    }
}