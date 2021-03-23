using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Users.DataAccess
{
    public interface IUserRepository : IWriteRepository<User>, IReadRepository<User>
    {
        Task<bool> IsExistingAsync(string email);
        Task<User> GetByIdWithRelationsAsync(string id);
        Task<User> GetByEmailAsync(string email, bool asNotTracking = true);
        Task<IEnumerable<User>> GetAllActiveAsync();
        Task<IEnumerable<User>> GetAllActiveByUserRoleAsync(UserRole userRole);
    }
}