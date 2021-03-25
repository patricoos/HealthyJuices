using System;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Shared.Dto
{
    public record UserDto
    {
        public string Id { get; init; }
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public UserRole Roles { get; init; }
        public string CompanyId { get; init; }
        public bool IsRemoved { get; init; }
        public DateTime Created { get; init; }
        public DateTime? LastModified { get; init; }
    }
}