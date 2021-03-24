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
            catch (CustomException e)
            {
                string logId = null;

                switch (e)
                {
                    case UnhandledException unhandledAxonException:
                        logId = await WriteLogAsync(context, e, logger);
                        break;
                }

                await WriteResponseAsync(context, e.Message, e.HttpStatusCode, logId);
            }
            catch (Exception ex)
            {
                var eid = await WriteLogAsync(context, ex, logger);
                await WriteResponseAsync(context, "There is something wrong with the remote server. Please contact administrator for more details.", HttpStatusCode.InternalServerError, eid);
            }
        }

        private async Task WriteResponseAsync(HttpContext context, string message, HttpStatusCode statusCode, string logId = null)
        {
            var responseMessage = logId != null ? $" {message}  {Environment.NewLine}  EID:  {logId} " : message;

            context.Response.StatusCode = (int)statusCode;

            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(responseMessage));
        }

        private static async Task<string> WriteLogAsync(HttpContext context, Exception exception, ILogger logger)
        {
            try
            {
                long? userId = null;
                var requestUrl = context.Request.Path + context.Request.QueryString;
                var requestBody = await GetRequestString(context);

                if (context.Items.ContainsKey("DriverId"))
                    if (long.TryParse(context.Items["DriverId"].ToString(), out var driverId))
                        userId = driverId;

                return await logger.LogAsync(LogSeverity.Unspecified, LogType.Api, exception, userId, null, requestUrl, requestBody);
            }
            catch (Exception ex)
            {
                Debug.Write(ex?.ToString());
                return null;
            }
        }

        private static async Task<string> GetRequestString(HttpContext context)
        {
            string body;

            context.Request.EnableBuffering();
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body);
            body = await reader.ReadToEndAsync();
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            return body;
        }

    }
}
