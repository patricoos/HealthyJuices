using System.Net;

namespace HealthyJuices.Common.Exceptions
{
    public class ConflictException : CustomException
    {
        public ConflictException(HttpStatusCode statusCode, string message, string uiMessage)
            : base(statusCode, message, uiMessage)
        {
        }

        public ConflictException(string message, string uiMessage)
            : base(HttpStatusCode.Conflict, message, uiMessage)
        {
        }

        public ConflictException(string message)
            : base(HttpStatusCode.Conflict, message, message)
        {
        }
    }
}
