﻿namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities
{
    public interface ISoftRemovableEntity : IEntity
    {
        bool IsRemoved { get; }
        void Remove();
    }
}