using HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories;

namespace HealthyJuices.Domain.Models.Unavailabilities.DataAccess
{
    public interface IUnavailabilityQueryBuilder : IQueryBuilder<Unavailability, IUnavailabilityQueryBuilder>
    {
    }
}