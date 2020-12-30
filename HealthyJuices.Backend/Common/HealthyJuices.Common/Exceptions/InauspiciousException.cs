using System.Net;

namespace HealthyJuices.Common.Exceptions
{
    public class InauspiciousException : CustomException
    {
        public InauspiciousException(HttpStatusCode statusCode, string message, string uiMessage)
            : base(statusCode, message, uiMessage)
        {
        }

        public InauspiciousException(string message, string uiMessage)
            : base(HttpStatusCode.Conflict, message, uiMessage)
        {
        }

        public InauspiciousException(string message)
            : base(HttpStatusCode.Conflict, message, message)
        {
        }
    }
}
