using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Logs.DataAccess;

namespace HealthyJuices.Persistence.MongoDb.Repositories
{
    public class LogRepository: BaseRepository<Log>, ILogRepository
    {
        public LogRepository(IMongoDbClient client) : base(client.Logs)
        {
        }
    }
}