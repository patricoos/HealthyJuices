using System;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities
{
    public interface IModifiableEntity : IEntity
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
    }
}