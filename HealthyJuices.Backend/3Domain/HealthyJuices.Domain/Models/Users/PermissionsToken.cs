using System;
using System.Collections.Generic;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Abstraction.DataAccess.Entities;

namespace HealthyJuices.Domain.Models.Users
{
    public class PermissionsToken : ValueObject
    {
        public string Token { get; private init; }
        public DateTime? Expiration { get; private init; }

        protected PermissionsToken() { }

        public PermissionsToken(ITimeProvider timeProvider, string token, DateTime date)
        {
            Token = token ?? throw new BadRequestException($"{nameof(Token)} can not be null");
            Expiration = date <= timeProvider.UtcNow ? throw new BadRequestException($"{nameof(PermissionsToken)} {nameof(Expiration)} must be from feature") : date;
        }

        public void CheckValidity(ITimeProvider timeProvider, string token)
        {
            if (Expiration <= timeProvider.UtcNow)
                throw new BadRequestException("Token Expiration Time Is Up");

            if (string.IsNullOrWhiteSpace(token) || Token != token)
                throw new BadRequestException("Invalid Token");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Token;
            yield return Expiration;
        }
    }
}