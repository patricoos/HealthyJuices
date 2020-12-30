using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Logs.DataAccess
{
    public interface ILogRepository : IPersistableRepository<Log>, IQueryableRepository<Log, ILogQueryBuilder>
    {
    }
}