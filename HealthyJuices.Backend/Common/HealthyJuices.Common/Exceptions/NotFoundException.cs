using System.Net;

namespace HealthyJuices.Common.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message)
            : this(message, message)
        {

        }

        public NotFoundException(string message, string uiMessage)
            : base(HttpStatusCode.NotFound, message, uiMessage)
        {
        }
    }
}