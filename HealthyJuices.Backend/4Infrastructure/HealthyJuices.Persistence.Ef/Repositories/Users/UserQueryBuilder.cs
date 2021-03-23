using System.Linq;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Persistence.Ef.Repositories.Users
{
    public class UserQueryBuilder : QueryBuilder<User, UserQueryBuilder>
    {
        public UserQueryBuilder(IQueryable<User> query) : base(query)
        {
        }


        public UserQueryBuilder ByEmail(string email)
        {
            Query = Query.Where(x => x.Email == email);
            return this;
        }

        public UserQueryBuilder ByUserRole(UserRole role)
        {
            Query = Query.Where(x => x.Roles.HasFlag(role));
            return this;
        }

        public UserQueryBuilder IsActive()
        {
            Query = Query.Where(x => x.IsActive == true);
            return this;
        }

        public UserQueryBuilder IncludeCompany()
        {
            Query = Query.Include(x => x.Company);
            return this;
        }

        public UserQueryBuilder IsNotRemoved()
        {
            Query = Query.Where(x => x.IsRemoved == false);
            return this;
        }
    }
}