using System;
using System.Threading.Tasks;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Services
{
    public interface ILogger
    {
        Task<long> LogInfoAsync(LogType type, string message, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<long> LogWarningAsync(LogType type, string message, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<long> LogErrorAsync(LogType type, string message, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<long> LogCriticalAsync(LogType type, string message, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<long> LogUnspecifiedAsync(LogType type, string message, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<long> LogInfoAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<long> LogWarningAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<long> LogErrorAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<long> LogCriticalAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);

        Task<long> LogUnspecifiedAsync(LogType type, Exception ex, long? userId = null, object sourceObject = null,
            string requestUrl = null, string requestBody = null);
    }
}