using System.Collections.Generic;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;

namespace HealthyJuices.Domain.Models.Users
{
    public class Password : ValueObject
    {
        public string Text { get; private set; }
        public string Salt { get; private set; }

        protected Password() { }

        public Password(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 3)
                throw new BadRequestException($"Password is incorrect.");

            this.Salt = PasswordHelper.GenerateSalt();
            this.Text = PasswordHelper.HashPassword(password, this.Salt);
        }

        public bool CheckValidity(string password)
        {
            var hashedPassword = PasswordHelper.HashPassword(password, Salt);
            return string.CompareOrdinal(hashedPassword, this.Text) == 0;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Text;
            yield return Salt;
        }
    }
}