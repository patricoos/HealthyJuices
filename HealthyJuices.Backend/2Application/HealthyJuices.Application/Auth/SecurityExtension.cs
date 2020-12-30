using System.Security.Claims;

namespace HealthyJuices.Application.Auth
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
