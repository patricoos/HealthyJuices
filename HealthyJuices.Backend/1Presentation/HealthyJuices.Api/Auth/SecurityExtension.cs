using System;
using System.Linq;
using System.Security.Claims;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Api.Auth
{
    public static class SecurityExtension
    {
        public static long GetId(this ClaimsPrincipal principal)
        {
            var idClaimValue = principal.Identity.Name;

            if (int.TryParse(idClaimValue, out var id))
                return id;

            return -1;
        }
    }
}
