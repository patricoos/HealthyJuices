using System;
using System.Linq;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Unavailabilities
{
    public class UnavailabilityQueryBuilder : QueryBuilder<Unavailability, IUnavailabilityQueryBuilder>, IUnavailabilityQueryBuilder
    {
        public UnavailabilityQueryBuilder(IQueryable<Unavailability> query, ITimeProvider timeProvider) : base(query, timeProvider)
        {
        }

        public IUnavailabilityQueryBuilder BeforeDateTime(DateTime date)
        {
            Query = Query.Where(x => x.From <= date);
            return this;
        }

        public IUnavailabilityQueryBuilder AfterDateTime(DateTime date)
        {
            Query = Query.Where(x => x.To >= date);
            return this;
        }

        public IUnavailabilityQueryBuilder BetweenDateTimes(DateTime @from, DateTime to)
        {
            AfterDateTime(from);
            BeforeDateTime(to);
            return this;

        }
    }
}