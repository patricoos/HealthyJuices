using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Repositories
{
    public interface IQueryableRepository<TAggregateRootEntity, out TQueryBuilder>
        where TAggregateRootEntity : Entity, IAggregateRoot
        where TQueryBuilder : class, IQueryBuilder<TAggregateRootEntity, TQueryBuilder>
    {
        TQueryBuilder Query();
    }
}
