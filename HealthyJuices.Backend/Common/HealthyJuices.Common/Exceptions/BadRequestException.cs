using System.Net;

namespace HealthyJuices.Common.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message, params string[] messageParameters)
            : base(HttpStatusCode.BadRequest, message, messageParameters)
        {
        }
    }
}