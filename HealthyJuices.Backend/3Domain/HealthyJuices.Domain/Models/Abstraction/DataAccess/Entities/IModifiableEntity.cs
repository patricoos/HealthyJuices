using System;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities
{
    public interface IModifiableEntity : IEntity
    {
        DateTime DateCreated { get; init; }
        DateTime DateModified { get; set; }
    }
}