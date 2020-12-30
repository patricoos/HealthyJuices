using System;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Users
{
    public class User : Entity, IModifiableEntity, ISoftRemovableEntity, IAggregateRoot
    {

        public string Email { get; set; }
        public UserRole Roles { get; private set; }

        public string Password { get; private set; }
        public string PasswordSalt { get; private set; }


        public long? CompanyId { get; set; }
        public Company Company { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsRemoved { get; set; }

        //public string ResetPasswordToken { get; set; }
        //public DateTime? ResetPasswordTokenExpiration { get; set; }


        public User() { }


        public void Remove()
        {
            this.IsRemoved = true;
            this.DateModified = DateTime.UtcNow;
        }
    }
}