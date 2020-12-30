using System.Net;

namespace HealthyJuices.Common.Exceptions
{
    public class UnhandledException : CustomException
    {
        public UnhandledException(string message)
            : this(message, message)
        {

        }

        public UnhandledException(string message, string uiMessage)
            : base(HttpStatusCode.InternalServerError, message, uiMessage)
        {
        }

        public UnhandledException(HttpStatusCode statusCode, string message, string uiMessage)
            : base(statusCode, message, uiMessage)
        {
        }
    }
}