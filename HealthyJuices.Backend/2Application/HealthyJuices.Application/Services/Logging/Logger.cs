using System;
using System.Reflection;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Logs.DataAccess;
using HealthyJuices.Domain.Services;
using HealthyJuices.Shared.Enums;
using Newtonsoft.Json;

namespace HealthyJuices.Application.Services.Logging
{
    public class Logger : ILogger
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ILogRepository _logRepository;

        public Logger(ILogRepository repository, ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _logRepository = repository;
        }

        private async Task<long> LogAsync(Assembly assembly, LogSeverity severity, LogType type, string message, long? userId, object sourceObject, string requestUrl, string requestBody)
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

            await _logRepository.Insert(log).SaveChangesAsync();

            return log.Id;
        }

        public Task<long> LogInfoAsync(LogType type, string message, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return this.LogAsync(Assembly.GetCallingAssembly(), LogSeverity.Info, type, message, userId, sourceObject, requestUrl, requestBody);
        }

        public Task<long> LogWarningAsync(LogType type, string message, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return this.LogAsync(Assembly.GetCallingAssembly(), LogSeverity.Warning, type, message, userId, sourceObject, requestUrl, requestBody);
        }

        public Task<long> LogErrorAsync(LogType type, string message, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return this.LogAsync(Assembly.GetCallingAssembly(), LogSeverity.Error, type, message, userId, sourceObject, requestUrl, requestBody);
        }

        public Task<long> LogCriticalAsync(LogType type, string message, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return this.LogAsync(Assembly.GetCallingAssembly(), LogSeverity.Critical, type, message, userId, sourceObject, requestUrl, requestBody);
        }

        public Task<long> LogUnspecifiedAsync(LogType type, string message, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return this.LogAsync(Assembly.GetCallingAssembly(), LogSeverity.Unspecified, type, message, userId, sourceObject, requestUrl, requestBody);
        }

        public Task<long> LogInfoAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return this.LogAsync(Assembly.GetCallingAssembly(), LogSeverity.Info, type, ExceptionReader.Read(ex), userId, sourceObject, requestUrl, requestBody);
        }

        public Task<long> LogWarningAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return this.LogAsync(Assembly.GetCallingAssembly(), LogSeverity.Warning, type, ExceptionReader.Read(ex), userId, sourceObject, requestUrl, requestBody);
        }

        public Task<long> LogErrorAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return this.LogAsync(Assembly.GetCallingAssembly(), LogSeverity.Error, type, ExceptionReader.Read(ex), userId, sourceObject, requestUrl, requestBody);
        }

        public Task<long> LogCriticalAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return this.LogAsync(Assembly.GetCallingAssembly(), LogSeverity.Critical, type, ExceptionReader.Read(ex), userId, sourceObject, requestUrl, requestBody);
        }

        public Task<long> LogUnspecifiedAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null)
        {
            return this.LogAsync(Assembly.GetCallingAssembly(), LogSeverity.Unspecified, type, ExceptionReader.Read(ex), userId, sourceObject, requestUrl, requestBody);
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