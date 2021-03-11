using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Email { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public UserRole Roles { get; private set; }

        public Password Password { get; private set; }

        public long? CompanyId { get; private set; }
        public Company Company { get; private set; }

        public DateTime DateCreated { get; private init; }
        public DateTime DateModified { get; private set; }
        public bool IsRemoved { get; private set; }
        public bool IsActive { get; private set; }

        public string PermissionsToken { get; private set; }
        public DateTime? PermissionsTokenExpiration { get; private set; }
        public List<string> RolesList => this.Roles.GetFlags().Select(x => Enum.GetName(typeof(UserRole), x)).ToList();

        protected User() { }


        public User(string email, string password, params UserRole[] role)
        {
            this.DateCreated = DateTime.Now;
            this.IsActive = false;
            this.IsRemoved = false;

            this.SetEmail(email);
            this.Password = new Password(password);

            this.SetPassword(password);
            this.AddRoles(role);
            this.Update();
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
            this.Update();
        }

        public void SetPassword(string password)
        {
            this.Password = new Password(password);
            this.Update();
        }

        public void Activate()
        {
            this.IsActive = true;
            this.Update();
        }

        public void SetPermissionsToken(string token, DateTime date)
        {
            PermissionsToken = token;
            PermissionsTokenExpiration = date;
            this.Update();
        }

        public void ResetPermissionsToken()
        {
            PermissionsToken = null;
            PermissionsTokenExpiration = null;
            this.Update();
        }

        public void Remove()
        {
            this.Update();
            this.IsRemoved = true;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception($"User can not have an empty email.");

            if (!new EmailAddressAttribute().IsValid(email))
                throw new BadRequestException($"Email address is invalid.");

            this.Email = email;
            this.Update();
        }

        private void Update()
        {
            if (this.IsRemoved)
                throw new BadRequestException("This user is removed");

            this.DateModified = DateTime.UtcNow;
        }

    }
}