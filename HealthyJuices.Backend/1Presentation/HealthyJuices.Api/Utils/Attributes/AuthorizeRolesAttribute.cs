using System;
using System.Linq;
using HealthyJuices.Shared.Enums;
using Microsoft.AspNetCore.Authorization;

namespace HealthyJuices.Api.Utils.Attributes
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserRole[] allowedRoles)
        {
            var allowedRolesAsStrings = allowedRoles.Select(x => Enum.GetName(typeof(UserRole), x));
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}
