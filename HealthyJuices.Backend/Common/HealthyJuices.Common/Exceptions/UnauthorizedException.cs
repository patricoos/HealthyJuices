using System.Net;

namespace HealthyJuices.Common.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(string message, params string[] messageParameters)
            : base(HttpStatusCode.Unauthorized, message, messageParameters)
        {
        }
    }
}