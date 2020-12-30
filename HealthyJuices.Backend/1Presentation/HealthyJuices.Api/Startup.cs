using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using HealthyJuices.Api.Bootstrap;
using HealthyJuices.Api.Middlewares;
using Microsoft.Extensions.Configuration;

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


            services
                .RegisterDatabase(Configuration.GetConnectionString("Sql"))
                .RegisterRepositories()
                .RegisterApplicationControllers()
                .RegisterServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
