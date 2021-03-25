using System;
using System.Net;

namespace HealthyJuices.Common.Exceptions
{
    public abstract class BaseException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string[] Params { get; set; }

        protected BaseException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.HttpStatusCode = statusCode;
        }
    }
}
