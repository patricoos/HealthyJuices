using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using HealthyJuices.Api.Bootstrap;
using HealthyJuices.Api.Middlewares;
using HealthyJuices.Application.Auth;
using HealthyJuices.Application.Utils;
using HealthyJuices.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace HealthyJuices.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private string StdOutLogPath => "../ApiLogs/";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            try
            {
                Directory.CreateDirectory(StdOutLogPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            services.AddSimpleTokenProvider(options =>
            {
                options.Secret = HealthyJuicesConstants.LOCAL_ACCESS_TOKEN_SECRET;
                options.Expiration = TimeSpan.FromDays(8);
                options.Issuer = HealthyJuicesConstants.LOCAL_ACCESS_TOKEN_ISSUER;
            });

            services.AddTokenAuthentication(Configuration);

            services
                .RegisterDatabase(Configuration.GetConnectionString("Sql"))
                .RegisterRepositories()
                .RegisterBehaviours()
                .RegisterMediatR()
                .AddValidators()
                .RegisterProviders(Configuration);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "HealthyJuices Api", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(config => config.WithHeaders("X-Access-Token").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.MigrateDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.SeedDefaultData();
                app.UseSwagger(c =>
                {
                    c.SerializeAsV2 = true;
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
