using HealthyJuices.Common.Contracts;
using HealthyJuices.Domain.Models.Unavailabilities;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;

namespace HealthyJuices.Persistence.Ef.Repositories.Unavailabilities
{
    public class UnavailabilityRepository : PersistableRepository<Unavailability>, IUnavailabilityRepository
    {
        public UnavailabilityRepository(IDbContext context, ITimeProvider timeProvider) : base(context, timeProvider)
        {
        }

        public IUnavailabilityQueryBuilder Query()
        {
            return new UnavailabilityQueryBuilder(AggregateRootDbSet.AsQueryable(), TimeProvider);
        }
    }
}