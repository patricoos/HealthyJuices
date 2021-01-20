using System;
using System.Collections.Generic;
using System.Linq;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Common.Extensions;
using HealthyJuices.Domain.Models.Abstraction;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;
using HealthyJuices.Domain.Models.Companies;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Domain.Models.Users
{
    public class User : Entity, IModifiableEntity, ISoftRemovableEntity, IAggregateRoot
    {
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserRole Roles { get; private set; }

        public string Password { get; private set; }
        public string PasswordSalt { get; private set; }


        public long? CompanyId { get; set; }
        public Company Company { get; set; }

        public DateTime DateCreated { get; init; }
        public DateTime DateModified { get; set; }
        public bool IsRemoved { get;  set; }
        public bool IsActive { get; private set; }

        public string ResetPermissionsToken { get; set; }
        public DateTime? ResetPermissionsTokenExpiration { get; set; }
        public List<string> RolesList => this.Roles.GetFlags().Select(x => Enum.GetName(typeof(UserRole), x)).ToList();


        public User() { }


        public User(string email, string password, params UserRole[] role)
        {
            this.DateCreated = DateTime.Now;
            this.DateModified = DateTime.Now;
            this.IsActive = false;
            this.IsRemoved = false;

            this.Email = email;
            this.PasswordSalt = PasswordManager.GenerateSalt();
            this.Password = PasswordManager.HashPassword(password, this.PasswordSalt);

            SetPassword(password);
            AddRoles(role);
        }

        public User(string email, string password, string firstName, string lastName, params UserRole[] role) : this(email, password, role)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public User(string email, string password, string firstName, string lastName, Company company, params UserRole[] role) : this(email, password, firstName, lastName, role)
        {
            this.Company = company;
        }

        public void AddRoles(params UserRole[] roles)
        {
            foreach (var userRole in roles)
                this.Roles |= userRole;
        }
        
        public bool CheckPasswordValidity(string password)
        {
            var hashedPassword = PasswordManager.HashPassword(password, PasswordSalt);
            return string.CompareOrdinal(hashedPassword, this.Password) == 0;
        }

        public void SetPassword(string password)
        {
            this.PasswordSalt = PasswordManager.GenerateSalt();
            this.Password = PasswordManager.HashPassword(password, this.PasswordSalt);
        }

        public void Activate()
        {
            this.IsActive = IsRemoved ? throw new BadRequestException("This user is removed") : true;
        }

        public void Remove()
        {
            this.IsRemoved = true;
            this.DateModified = DateTime.UtcNow;
        }
    }
}