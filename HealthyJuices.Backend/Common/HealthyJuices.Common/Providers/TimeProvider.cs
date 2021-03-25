using System;
using HealthyJuices.Common.Contracts;

namespace HealthyJuices.Common.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}