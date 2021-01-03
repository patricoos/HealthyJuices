using System;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Shared.Dto
{
    public record UserDto(long Id, string Email, string firstName, string lastName, UserRole Roles, long? CompanyId, DateTime DateCreated, DateTime DateModified, bool IsRemoved);

    public record AddOrEditUserDto(long? Id, string Email, string Password, UserRole Roles, long? CompanyId);
}