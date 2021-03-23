using System.Collections.Generic;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Logs.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Logs
{
    public class LogRepository : BaseRepository<Log>, ILogWriteRepository
    {
        private LogQueryBuilder Query => new LogQueryBuilder(AggregateRootDbSet.AsQueryable());

        public LogRepository(IDbContext context) : base(context)
        {
        }
    }
}