using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Dto;

namespace HealthyJuices.Application.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(this User e) => new UserDto(e.Id, e.Email, e.FirstName, e.LastName, e.Roles, e.CompanyId, e.DateCreated.UtcDateTime, e.DateModified.UtcDateTime, e.IsRemoved);
    }
}