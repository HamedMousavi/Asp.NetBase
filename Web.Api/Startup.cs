using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Middlewares;
using Microsoft.Extensions.Configuration;
using Web.Api.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Web.Api.Logging;


namespace Web.Api
{

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
            //logger.Trace($"Config providers: {string.Join(", ", (configuration as IConfigurationRoot)?.Providers?.ToList())}");
        }


        public void ConfigureServices(IServiceCollection services)
        {
            // Microsoft, is there really not a sane way of injecting Logger in Program and using it here?!!
            var logger = Program.Logger; // todo: do something about this ugly piece of code. :-/

            services.Configure<CustomHeadersSettings>(configuration.GetSection(CustomHeadersSettings.ConfigFileSectionName));
            services.Configure<CorsSettings>(configuration.GetSection(CorsSettings.ConfigFileSectionName));

            services.AddCorsService();
            services.AddHealthChecks().AddCheck(LoggerHealthCheck.Name, new LoggerHealthCheck(logger), LoggerHealthCheck.Severity, LoggerHealthCheck.Tags);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCustomHeaders();

            app.UseRouting();

            app.UseCorsService();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var logger = context.RequestServices.GetService<ILogger>();
                    logger.Trace("Hello World!");
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
