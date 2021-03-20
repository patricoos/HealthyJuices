using System.Net;

namespace HealthyJuices.Common.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message, params string[] translationParams)
            : base(HttpStatusCode.BadRequest, message, translationParams)
        {
        }
    }
}