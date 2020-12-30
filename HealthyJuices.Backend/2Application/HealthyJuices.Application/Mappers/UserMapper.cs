using System.Linq;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Dto;

namespace Nexus.Application.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(this User e) => new UserDto(e.Id, e.Email, e.Roles, e.CompanyId, e.DateCreated, e.DateModified, e.IsRemoved);
    }
}