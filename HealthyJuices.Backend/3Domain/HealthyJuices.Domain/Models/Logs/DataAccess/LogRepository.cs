using System.Threading.Tasks;

namespace HealthyJuices.Domain.Models.Logs.DataAccess
{
    public interface ILogRepository
    {
        Task<string> Insert(Log entity);
    }
}