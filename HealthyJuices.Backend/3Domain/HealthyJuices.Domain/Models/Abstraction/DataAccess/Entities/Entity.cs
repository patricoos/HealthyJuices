using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities
{
     public abstract class Entity : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; protected set; }
        public DateTimeOffset Created { get; protected init; }
        public DateTimeOffset? LastModified { get; protected set; }

        //public string ModifiedBy { get; protected set; }
        //public string CreatedBy { get; protected set; }
    }
}