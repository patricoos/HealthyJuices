using System;
using System.Reflection;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Logs.DataAccess;
using HealthyJuices.Domain.Providers;
using HealthyJuices.Shared.Enums;
using Newtonsoft.Json;

namespace HealthyJuices.Application.Providers.Logging
{
    public class Logger : ILogger
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ILogWriteRepository _logWriteRepository;

        public Logger(ILogWriteRepository writeRepository, ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _logWriteRepository = writeRepository;
        }

        public async Task<string> LogAsync(LogSeverity severity, LogType type, string message, long? userId, object sourceObject, string requestUrl, string requestBody)
        {
            var log = new Log
            {
                Date = _timeProvider.UtcNow,
                Message = message,
                RequestBody = requestBody,
                RequestUrl = requestUrl,
                Severity = severity,
                SourceObject = this.ParseSourceObject(sourceObject),
                Type = type,
                UserId = userId
            };

            await _logWriteRepository.Insert(log).SaveChangesAsync();

            return log.Id;
        }

        public async Task<string> LogAsync(LogSeverity severity, LogType type, Exception ex, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return await this.LogAsync(severity, type, ExceptionHelper.Read(ex), userId, sourceObject, requestUrl, requestBody);
        }

        private string ParseSourceObject(object sourceObject)
        {
            if (sourceObject == null)
                return null;

            return sourceObject.GetType().IsValueType
                ? sourceObject.ToString()
                : JsonConvert.SerializeObject(sourceObject);
        }
    }
}