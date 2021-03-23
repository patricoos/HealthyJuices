using System;

namespace HealthyJuices.Common.Contracts
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}