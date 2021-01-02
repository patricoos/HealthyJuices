using System;
using HealthyJuices.Common.Contracts;

namespace HealthyJuices.Common.Services
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;

        public TimeZoneInfo LocalTimeZone => TimeZoneInfo.Local;

        public DateTime ToLocalDateTime(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Local)
                return dateTime;

            if (dateTime.Kind != DateTimeKind.Utc)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
            return TimeZoneInfo.ConvertTime(dateTime, LocalTimeZone);
        }

        public DateTime ToUtcDateTime(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime;

            if (dateTime.Kind != DateTimeKind.Local)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            }
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc);
        }
    }
}