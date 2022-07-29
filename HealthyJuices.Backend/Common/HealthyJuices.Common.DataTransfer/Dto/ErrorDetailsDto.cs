using System.Collections.Generic;
using System.Net;

namespace HealthyJuices.Shared.Dto
{

    public class ErrorDetailsDto
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
        public string Details { get; }
        public string LogId { get; }
        public IDictionary<string, string[]> Errors { get; }

        public ErrorDetailsDto(HttpStatusCode statusCode, string message, string details, string logId, IDictionary<string, string[]> errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
            LogId = logId;

            Errors = errors;
        }
    }
}