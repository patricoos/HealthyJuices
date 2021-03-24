using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Logs.DataAccess
{
    public interface ILogRepository : IWriteRepository<Log>, IReadRepository<Log>
    {
    }
}