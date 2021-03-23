using System;
using System.Net;

namespace HealthyJuices.Common.Exceptions
{
    public abstract class CustomException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string[] TranslationParams { get; set; }

        protected CustomException(HttpStatusCode statusCode, string message, params string[] translationParams)
            : base(message)
        {
            this.HttpStatusCode = statusCode;
            TranslationParams = translationParams;
        }

        protected CustomException(HttpStatusCode statusCode, Exception exception, params string[] translationParams)
            : this(statusCode, exception.Message, translationParams)
        {
        }
    }
}
