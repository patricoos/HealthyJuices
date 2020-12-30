using System;

namespace HealthyJuices.Shared.Enums
{
    [Flags]
    public enum UserRole
    {
        None = 0,
        Customer = 1,
        BusinessOwner = 2
    }
}