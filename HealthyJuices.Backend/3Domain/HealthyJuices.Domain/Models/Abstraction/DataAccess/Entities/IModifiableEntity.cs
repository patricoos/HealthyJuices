using System;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities
{
    public interface IModifiableEntity : IEntity
    {
        DateTime DateCreated { get; }
        DateTime DateModified { get; }

        // TODO: add setting to dbcontext
    }
}