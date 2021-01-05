using System;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Unavailabilities.DataAccess
{
    public interface IUnavailabilityQueryBuilder : IQueryBuilder<Unavailability, IUnavailabilityQueryBuilder>
    {
        IUnavailabilityQueryBuilder BeforeDateTime(DateTime date);
        IUnavailabilityQueryBuilder AfterDateTime(DateTime date);
        IUnavailabilityQueryBuilder BetweenDateTimes(DateTime from, DateTime to);
    }
}