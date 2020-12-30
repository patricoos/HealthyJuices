using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Users.DataAccess
{
    public interface IUserRepository : IPersistableRepository<User>, IQueryableRepository<User, IUserQueryBuilder>
    {
        Task<bool> IsExistingAsync(string email);
        Task<bool> IsExistingAsync(long id);
    }
}