using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Users
{
    public class UserRepository : PersistableRepository<User>, IUserRepository
    {
        public UserRepository(IDbContext context, ITimeProvider timeProvider) : base(context, timeProvider)
        {
        }

        public IUserQueryBuilder Query()
        {
            return new UserQueryBuilder(AggregateRootDbSet.AsQueryable(), TimeProvider);
        }

        public async Task<bool> IsExistingAsync(string email)
        {
            var result = await Query()
                .ByEmail(email)
                .IsNotRemoved()
                .AnyAsync();

            return result;
        }
    }
}