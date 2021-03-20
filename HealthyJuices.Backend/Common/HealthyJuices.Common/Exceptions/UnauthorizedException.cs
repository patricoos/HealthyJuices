using System.Net;

namespace HealthyJuices.Common.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(string message, params string[] translationParams)
            : base(HttpStatusCode.Unauthorized, message, translationParams)
        {
        }
    }
}