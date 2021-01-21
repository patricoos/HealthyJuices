using System;
using System.Collections.Generic;
using System.Text;
using HealthyJuices.Api.Bootstrap.DataSeed;
using HealthyJuices.Application.Services;
using HealthyJuices.Application.Services.Logging;
using HealthyJuices.Common;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Services;
using HealthyJuices.Domain.Models.Companies.DataAccess;
using HealthyJuices.Domain.Models.Logs.DataAccess;
using HealthyJuices.Domain.Models.Orders.DataAccess;
using HealthyJuices.Domain.Models.Products.DataAccess;
using HealthyJuices.Domain.Models.Unavailabilities.DataAccess;
using HealthyJuices.Domain.Models.Users.DataAccess;
using HealthyJuices.Domain.Services;
using HealthyJuices.Mailing;
using HealthyJuices.Persistence.Ef;
using HealthyJuices.Persistence.Ef.Repositories.Companies;
using HealthyJuices.Persistence.Ef.Repositories.Logs;
using HealthyJuices.Persistence.Ef.Repositories.Orders;
using HealthyJuices.Persistence.Ef.Repositories.Products;
using HealthyJuices.Persistence.Ef.Repositories.Unavailabilities;
using HealthyJuices.Persistence.Ef.Repositories.Users;
using HealthyJuices.Shared.Enums;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
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
            @this.AddScoped<AuthorizationService>();
            @this.AddScoped<OrdersService>();
            @this.AddScoped<UnavailabilitiesService>();
            @this.AddScoped<UsersService>();
            @this.AddScoped<CompaniesService>();
            @this.AddScoped<ProductsService>();
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
            @this.AddScoped<IOrderRepository, OrderRepository>();
            @this.AddScoped<IUnavailabilityRepository, UnavailabilityRepository>();
            @this.AddScoped<ICompanyRepository, CompanyRepository>();
            @this.AddScoped<IProductRepository, ProductRepository>();

            return @this;
        }

        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder @this)
        {
            using var serviceScope = @this.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<HealthyJuicesDbContext>();

            context.Database.Migrate();

            return @this;
        }

        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(HealthyJuicesConstants.LOCAL_ACCESS_TOKEN_SECRET)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Anyone", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.AuthenticationSchemes = new List<string> { "Bearer" };
                });
                options.AddPolicy(Enum.GetName(typeof(UserRole), UserRole.Customer), builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.AuthenticationSchemes = new List<string> { "Bearer" };
                    builder.AddRequirements(new RolesAuthorizationRequirement(new[] { UserRole.Customer.ToString() }));
                });
                options.AddPolicy(Enum.GetName(typeof(UserRole), UserRole.BusinessOwner), builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.AuthenticationSchemes = new List<string> { "Bearer" };
                    builder.AddRequirements(new RolesAuthorizationRequirement(new[] { UserRole.BusinessOwner.ToString() }));
                });

                options.DefaultPolicy = options.GetPolicy("Anyone");
            });

            return services;
        }

        public static IApplicationBuilder SeedDefaultData(this IApplicationBuilder @this)
        {
            using var serviceScope = @this.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<HealthyJuicesDbContext>();

            var seeders = new List<IDataSeeder>()
                {
                        new ProductDataSeeder(),
                        new CompanyDataSeeder(),
                        new UserDataSeeder(),
                        new OrderDataSeeder()

                };

            foreach (var dataSeeder in seeders)
            {
                dataSeeder.Seed(context);
            }

            return @this;
        }
    }
}