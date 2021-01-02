using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using HealthyJuices.Api.Bootstrap.DataSeed;
using HealthyJuices.Application.Auth;
using HealthyJuices.Application.Controllers;
using HealthyJuices.Application.Services.Logging;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Services;
using HealthyJuices.Domain.Models.Logs.DataAccess;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Domain.Services;
using HealthyJuices.Mailing;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Persistence.Ef.Repositories.Logs;
using HealthyJuices.Persistence.Ef.Repositories.Users;
using HealthyJuices.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HealthyJuices.Api.Bootstrap
{
    public static class StartupExtensions
    {
        public static IServiceCollection RegisterApplicationControllers(this IServiceCollection @this)
        {
            @this.AddScoped<AuthorizationController>();
            return @this;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection @this, IConfiguration config)
        {
            @this.AddScoped<IMailer>(x => new Mailer(
                config["smtp:smtpServer"],
                config["smtp:smtpUser"],
                config["smtp:smtpPassword"],
                config["smtp:smtpMailFrom"],
                int.Parse(config["smtp:port"])
            ));

            @this.AddScoped<ILogger, Logger>();
            @this.AddScoped<ITimeProvider, TimeProvider>();
            @this.AddScoped<EmailService>();

            return @this;
        }

        public static IServiceCollection RegisterDatabase(this IServiceCollection @this, string connectionString)
        {
            @this.AddDbContext<HealthyJuicesDbContext>(options =>
                options.UseSqlServer(connectionString));

            return @this;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection @this)
        {
            @this.AddScoped<IDbContext, HealthyJuicesDbContext>();

            @this.AddScoped<ILogRepository, LogRepository>();
            @this.AddScoped<IUserRepository, UserRepository>();

            return @this;
        }

        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder @this)
        {
            using var serviceScope = @this.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<HealthyJuicesDbContext>();

            context.Database.Migrate();

            return @this;
        }

        public static IApplicationBuilder SeedDefaultData(this IApplicationBuilder @this)
        {
            using var serviceScope = @this.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<HealthyJuicesDbContext>();

            var seeders = new List<IDataSeeder>()
                {
                        new UserDataSeeder(),
                };

            foreach (var dataSeeder in seeders)
            {
                dataSeeder.Seed(context);
            }

            return @this;
        }
    }
}