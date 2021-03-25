using System;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities
{
    public interface IEntity
    {
        string Id { get; }
        DateTimeOffset Created { get; }
        DateTimeOffset? LastModified { get; }

        public string LastModifiedBy { get; }
        public string CreatedBy { get; }
    }
}