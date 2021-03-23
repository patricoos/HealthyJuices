using System;
using System.Linq;
using HealthyJuices.Domain.Models.Unavailabilities;

namespace HealthyJuices.Persistence.Ef.Repositories.Unavailabilities
{
    public class UnavailabilityQueryBuilder : QueryBuilder<Unavailability, UnavailabilityQueryBuilder>
    {
        public UnavailabilityQueryBuilder(IQueryable<Unavailability> query) : base(query)
        {
        }

        public UnavailabilityQueryBuilder BeforeDateTime(DateTime date)
        {
            Query = Query.Where(x => x.From <= date);
            return this;
        }

        public UnavailabilityQueryBuilder AfterDateTime(DateTime date)
        {
            Query = Query.Where(x => x.To >= date);
            return this;
        }

        public UnavailabilityQueryBuilder BetweenDateTimes(DateTime @from, DateTime to)
        {
            AfterDateTime(from);
            BeforeDateTime(to);
            return this;
        }
    }
}