using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Unavailabilities.DataAccess
{
    public interface IUnavailabilityRepository : IPersistableRepository<Unavailability>, IQueryableRepository<Unavailability, IUnavailabilityQueryBuilder>
    {
        
    }
}