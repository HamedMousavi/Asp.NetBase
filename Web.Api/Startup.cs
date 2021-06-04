using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Middlewares;
using Microsoft.Extensions.Configuration;
using Web.Api.Settings;
using Web.Api.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;


namespace Web.Api
{

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var corsSettings = configuration.GetSection(CorsSettings.ConfigFileSectionName).Get<CorsSettings>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsSettings.PolocyName, builder => {
                    builder
                    .WithOrigins(corsSettings.WhiteList.ToArray())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    /*.SetIsOriginAllowed(origin => { return false; })*/;
                });
            });

            services.AddHealthChecks().AddCheck(LoggerHealthCheck.Name, new LoggerHealthCheck(), LoggerHealthCheck.Severity, LoggerHealthCheck.Tags);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomHeaders(configuration.GetSection(CustomHeadersSettings.ConfigFileSectionName).Get<CustomHeadersSettings>());

            app.UseRouting();

            app.UseCors(CorsSettings.PolocyName);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });
        }


        private readonly IConfiguration configuration;

    }

}
