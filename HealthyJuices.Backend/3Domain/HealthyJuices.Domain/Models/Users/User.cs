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


        public User(string email, string password, UserRole role)
        {
            this.Email = email;
            this.PasswordSalt = PasswordManager.GenerateSalt();
            this.Password = PasswordManager.HashPassword(password, this.PasswordSalt);
            this.DateCreated = DateTime.Now;
            this.DateModified = DateTime.Now;
            AddRoles(role);
        }

        public void AddRoles(params UserRole[] roles)
        {
            foreach (var userRole in roles)
            {
                this.Roles |= userRole;
            }
        }


        public void Remove()
        {
            this.IsRemoved = true;
            this.DateModified = DateTime.UtcNow;
        }
    }
}