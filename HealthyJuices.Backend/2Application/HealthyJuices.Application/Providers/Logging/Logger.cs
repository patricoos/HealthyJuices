using System;
using System.Threading.Tasks;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Logs;
using HealthyJuices.Domain.Models.Logs.DataAccess;
using HealthyJuices.Domain.Providers;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Application.Providers.Logging
{
    public class Logger : ILogger
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogRepository _logRepository;
        private readonly ICurrentUserProvider _currentUserProvider;

        public Logger(ILogRepository repository, IDateTimeProvider dateTimeProvider, ICurrentUserProvider currentUserProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _currentUserProvider = currentUserProvider;
            _logRepository = repository;
        }

        public async Task<string> LogAsync(LogSeverity severity, LogType type, string message,  string path = null, string body = null)
        {
            var log = new Log
            {
                Date = _dateTimeProvider.UtcNow,
                Message = message,
                RequestBody = body,
                RequestUrl = path,
                Severity = severity,
                Type = type,
                UserId = _currentUserProvider.UserId
            };

            await _logRepository.Insert(log);
            return log.Id;
        }

        public async Task<string> LogAsync(LogSeverity severity, LogType type, Exception ex, string path = null, string body = null)
        {
            return await this.LogAsync(severity, type, ExceptionHelper.Read(ex), path, body);
        }
    }
}