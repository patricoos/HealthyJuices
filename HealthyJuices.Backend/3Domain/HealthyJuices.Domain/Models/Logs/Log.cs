using System;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Logs
{
    public class Log : Entity, IAggregateRoot
    {
        public DateTime Date { get; set; }
        public LogSeverity Severity { get; set; }
        public LogType Type { get; set; }
        public string Message { get; set; }
        public long? UserId { get; set; }
        public string SourceObject { get; set; }
        public string RequestUrl { get; set; }
        public string RequestBody { get; set; }

        public Log() { }
    }
}