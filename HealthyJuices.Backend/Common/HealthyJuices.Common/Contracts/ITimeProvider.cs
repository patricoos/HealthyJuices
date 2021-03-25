using System;

namespace HealthyJuices.Common.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}