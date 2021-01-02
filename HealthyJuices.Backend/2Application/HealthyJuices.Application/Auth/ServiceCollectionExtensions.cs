using System;
using Microsoft.Extensions.DependencyInjection;

namespace HealthyJuices.Application.Auth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSimpleTokenProvider(this IServiceCollection services, Action<TokenProviderOptions> configureOptions)
        {
            var options = new TokenProviderOptions() { Expiration = TimeSpan.FromDays(30) };

            configureOptions?.Invoke(options);

            if (string.IsNullOrEmpty(options.Secret))
                throw new ArgumentException("Secret must be provided in options");

            var provider = new SimpleTokenProvider(options.Expiration, options.Secret);

            provider.Issuer = options.Issuer;

            services.AddSingleton(provider);

            return services;
        }
    }
}
