using System;
using HealthyJuices.Common.Contracts;

namespace HealthyJuices.Common.Services
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}