using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Logs.DataAccess
{
    public interface ILogWriteRepository : IWriteRepository<Log>
    {
    }
}