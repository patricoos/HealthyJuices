using System;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Shared.Dto
{
    public record UserDto(string Id, string Email, string firstName, string lastName, UserRole Roles, long? CompanyId, bool IsRemoved, DateTime DateCreated, DateTime? DateModified);

    public record AddOrEditUserDto(string Id, string Email, string Password, UserRole Roles, long? CompanyId);
}