using System;

namespace HealthyJuices.Application.Auth
{
    public class TokenProviderOptions
    {
        public TimeSpan Expiration { get; set; }
        public string Secret { get; set; }
        public string Issuer { get; set; }
    }
}
