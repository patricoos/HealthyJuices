using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Logs.DataAccess;
using Nest;

namespace HealthyJuices.Persistence.Elasticsearch.Repositories
{
    public class LogElasticRepository : ILogRepository
    {
        private const string IndexName = "Logs";

        private readonly IElasticClient _elasticClient;
        public LogElasticRepository(IElasticClient elasticClient)
        {
            this._elasticClient = elasticClient;
        }

        public async Task<string> Insert(Log entity)
        {
            var result = await _elasticClient.IndexAsync<Log>(entity, x => x.Index(IndexName));
            return result.Id;
        }
    }
}