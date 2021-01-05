using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Users.DataAccess
{
    public interface IUserQueryBuilder : IQueryBuilder<User, IUserQueryBuilder>
    {
        IUserQueryBuilder ByEmail(string email);
        IUserQueryBuilder ByUserRole(UserRole role);
        IUserQueryBuilder IsActive();
        IUserQueryBuilder IncludeCompany();
        IUserQueryBuilder IsNotRemoved();
    }
}