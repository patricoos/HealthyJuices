using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;
using HealthyJuices.Api.Bootstrap;
using HealthyJuices.Api.Middlewares;
using HealthyJuices.Application.Auth;
using HealthyJuices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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
                .RegisterApplicationControllers()
                .RegisterServices(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(config => config.WithHeaders("X-Access-Token").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.MigrateDatabase();
            app.SeedDefaultData();
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
