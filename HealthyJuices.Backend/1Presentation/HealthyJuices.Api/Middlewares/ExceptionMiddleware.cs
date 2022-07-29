using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Providers;
using HealthyJuices.Shared.Dto;
using HealthyJuices.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HealthyJuices.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger logger)
        {
            try
            {
                await _next(context);
            }
            catch (BaseException e)
            {
                await WriteResponseAsync(context, e.Message, e.HttpStatusCode);
            }
            catch (Exception ex)
            {
                var eid = await WriteLogAsync(context, ex, logger);
                await WriteResponseAsync(context, "Server not responding.", HttpStatusCode.InternalServerError, eid);
            }
        }

        private async Task WriteResponseAsync(HttpContext context, string message, HttpStatusCode statusCode, string logId = null, IDictionary<string, string[]> errors = null, string details = null)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var error = new ErrorDetailsDto(statusCode, message, details, logId, errors);
            var body = JsonConvert.SerializeObject(error, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            await context.Response.WriteAsync(body);
        }

        private static async Task<string> WriteLogAsync(HttpContext context, Exception exception, ILogger logger)
        {
            try
            {
                var url = context.Request.Path + context.Request.QueryString;
                var body = await GetRequestString(context);

                var logId = await logger.LogAsync(LogSeverity.Unspecified, LogType.Api, exception, url, body);
                return logId;
            }
            catch (Exception ex)
            {
                Debug.Write(ex?.ToString());
                return null;
            }
        }

        private static async Task<string> GetRequestString(HttpContext context)
        {
            context.Request.EnableBuffering();
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            return body;
        }
    }
}
