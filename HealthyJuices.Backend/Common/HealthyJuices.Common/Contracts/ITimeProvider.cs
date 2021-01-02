using System;

namespace HealthyJuices.Common.Contracts
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }

        TimeZoneInfo LocalTimeZone { get; }

        DateTime ToLocalDateTime(DateTime dateTime);
        DateTime ToUtcDateTime(DateTime dateTime);
    }
}