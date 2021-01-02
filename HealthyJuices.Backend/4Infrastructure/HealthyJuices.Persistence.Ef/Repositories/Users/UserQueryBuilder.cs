using System.Linq;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Persistence.Ef.Repositories.Users
{
    public class UserQueryBuilder : QueryBuilder<User, IUserQueryBuilder>, IUserQueryBuilder
    {
        public UserQueryBuilder(IQueryable<User> query, ITimeProvider timeProvider) : base(query, timeProvider)
        {
        }


        public IUserQueryBuilder ByEmail(string email)
        {
            Query = Query.Where(x => x.Email == email);
            return this;
        }

        public IUserQueryBuilder ByUserRole(UserRole role)
        {
            Query = Query.Where(x => x.Roles.HasFlag(role));
            return this;
        }

        public IUserQueryBuilder IsActive()
        {
            Query = Query.Where(x => x.IsActive == true);
            return this;
        }

        public IUserQueryBuilder IsNotRemoved()
        {
            Query = Query.Where(x => x.IsRemoved == false);
            return this;
        }
    }
}