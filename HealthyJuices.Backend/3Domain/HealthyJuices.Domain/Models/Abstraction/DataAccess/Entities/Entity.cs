namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities
{
     public abstract class Entity : IEntity
    {
        public long Id { get; set; }
    }
}