using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Shared.Dto.Auth;
using Microsoft.IdentityModel.Tokens;

namespace HealthyJuices.Application.Auth
{
    public class SimpleTokenProvider
    {
        public string Secret { get; private set; }
        public TimeSpan ExpirationTime { get; private set; }
        public string Issuer { get; set; }

        public SimpleTokenProvider(TimeSpan expiration, string secret)
        {
            ExpirationTime = expiration;
            Secret = secret;
        }

        public string Create(string userId, IDateTimeProvider clock, IEnumerable<string> roles = null, params Claim[] claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userId)
            });

            if (roles != null)
                foreach (var role in roles)
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));

            if (claims != null)
                foreach (var claim in claims)
                    identity.AddClaim(claim);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = clock.UtcNow.Add(ExpirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = clock.UtcNow,
                Issuer = Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public RefreshToken CreateRefreshToken(string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}
