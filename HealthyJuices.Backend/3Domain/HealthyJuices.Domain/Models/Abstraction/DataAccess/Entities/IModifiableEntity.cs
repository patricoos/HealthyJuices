using System;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities
{
    public interface IModifiableEntity : IEntity
    {
        DateTimeOffset DateCreated { get; }
        DateTimeOffset DateModified { get; }

        // TODO: add setting to dbcontext
    }
}