using System;
using System.Linq;
using HealthyJuices.Domain.Models.Users;
using HealthyJuices.Shared.Enums;
using HealthyJuices.Tests.EndToEnd.Extensions;
using HealthyJuices.Tests.EndToEnd.SeedTestData.Abstraction;

namespace HealthyJuices.Tests.EndToEnd.SeedTestData
{
    public class UserBuilder : TestDataBuilder<User>
    {
        public static UserBuilder Create() => new UserBuilder();

        public UserBuilder WithEmail(string email)
        {
            base.Entity.SetProperty(x => x.Email, email);
            return this;
        }

        public UserBuilder WithPassword(string password)
        {
            var salt = PasswordHelper.GenerateSalt();
            var pass = (Password) Activator.CreateInstance(typeof(Password), true);
            pass.SetProperty(x => x.Salt, salt);
            pass.SetProperty(x => x.Text, PasswordHelper.HashPassword(password, salt));

            base.Entity.SetProperty(x => x.Password, pass);
            return this;
        }

        public UserBuilder WithPermissionsToken(string token, DateTime expiration)
        {
            base.Entity.SetProperty(p => p.PermissionsToken, token);
            base.Entity.SetProperty(p => p.PermissionsTokenExpiration, expiration);
            return this;
        }

        public UserBuilder WithRole(params UserRole[] roles)
        {
            var role = UserRole.None;
            foreach (var userRole in roles)
                role |= userRole;

            base.Entity.SetProperty(p => p.Roles, role);

            return this;
        }

        public UserBuilder Active()
        {
            base.Entity.SetProperty(p => p.IsActive, true);
            return this;
        }

        public UserBuilder UnActive()
        {
            base.Entity.SetProperty(p => p.IsActive, false);
            return this;
        }

        public UserBuilder Removed()
        {
            base.Entity.SetProperty(p => p.IsRemoved, true);
            return this;
        }
    }
}