using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Providers;
using HealthyJuices.Shared.Enums;
using Microsoft.AspNetCore.Http;

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
                await WriteLogAsync(context, ex, logger);
                await WriteResponseAsync(context, "Server not responding.", HttpStatusCode.InternalServerError);
            }
        }

        private async Task WriteResponseAsync(HttpContext context, string message, HttpStatusCode statusCode)
        {
            context.Response.StatusCode = (int)statusCode;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(message));
        }

        private static async Task WriteLogAsync(HttpContext context, Exception exception, ILogger logger)
        {
            try
            {
                var url = context.Request.Path + context.Request.QueryString;
                var body = await GetRequestString(context);

                await logger.LogAsync(LogSeverity.Unspecified, LogType.Api, exception, null, url, body);
            }
            catch (Exception ex)
            {
                Debug.Write(ex?.ToString());
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
