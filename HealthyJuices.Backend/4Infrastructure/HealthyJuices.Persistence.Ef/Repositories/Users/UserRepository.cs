using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories.Users
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public UserRepository(IDbContext context) : base(context)
        {
        }
        public async Task<bool> IsExistingAsync(string email)
        {
            var result = await Query
                .ByEmail(email)
                .IsNotRemoved()
                .AnyAsync();

            return result;
        }

        public async Task<User> GetByIdWithRelationsAsync(string id)
        {
            var result = await Query
                .ById(id)
                .IncludeCompany()
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<User> GetByEmailAsync(string email, bool asNotTracking = true)
        {
            if (asNotTracking)
                return await Query
                    .ByEmail(email)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            return await Query
                .ByEmail(email)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllActiveAsync()
        {
            return await Query
                .IsNotRemoved()
                .IsActive()
                .ToListAsync();

        }

        public async Task<IEnumerable<User>> GetAllActiveByUserRoleAsync(UserRole userRole)
        {
            return await Query
                .ByUserRole(userRole)
                .IsNotRemoved()
                .IsActive()
                .ToListAsync();
        }
    }
}