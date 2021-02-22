using System;
using System.Threading.Tasks;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Providers
{
    public interface ILogger
    {
        Task<string> LogInfoAsync(LogType type, string message, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<string> LogWarningAsync(LogType type, string message, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<string> LogErrorAsync(LogType type, string message, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<string> LogCriticalAsync(LogType type, string message, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<string> LogUnspecifiedAsync(LogType type, string message, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<string> LogInfoAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<string> LogWarningAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<string> LogErrorAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<string> LogCriticalAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<string> LogUnspecifiedAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);
    }
}