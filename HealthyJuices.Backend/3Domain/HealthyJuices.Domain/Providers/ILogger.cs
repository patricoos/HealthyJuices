using System;
using System.Threading.Tasks;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Providers
{
    public interface ILogger
    {
        Task<string> LogAsync(LogSeverity severity, LogType type, string message, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null);
        Task<string> LogAsync(LogSeverity severity, LogType type, Exception ex, long? userId = null, object sourceObject = null, string requestUrl = null, string requestBody = null);
    }
}