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
    }
}