﻿using System;
using System.Threading.Tasks;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Providers
{
    public interface ILogger
    {
        Task<string> LogAsync(LogSeverity severity, LogType type, string message, string userId = null, string path = null, string body = null);
        Task<string> LogAsync(LogSeverity severity, LogType type, Exception ex, string userId = null, string path = null, string body = null);
    }
}