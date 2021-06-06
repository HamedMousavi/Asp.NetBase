using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Middlewares;
using Microsoft.Extensions.Configuration;
using Web.Api.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Web.Api.Middlewares;


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
            // Microsoft, is there really not a sane way of injecting Logger in Program and using it here?!!
            var logger = Program.Logger; // todo: do something about this ugly piece of code. :-/
            //logger.Trace($"Config providers: {string.Join(", ", (configuration as IConfigurationRoot)?.Providers?.ToList())}");

            services.Configure<CustomHeadersSettings>(configuration.GetSection(CustomHeadersSettings.ConfigFileSectionName));
            services.Configure<CorsSettings>(configuration.GetSection(CorsSettings.ConfigFileSectionName));

            services.AddCorsService();
            services.AddHealthChecks().AddCheck(LoggerHealthCheck.Name, new LoggerHealthCheck(logger), LoggerHealthCheck.Severity, LoggerHealthCheck.Tags);

            services.AddControllers(o =>
            {
                o.OutputFormatters.Clear();
                o.OutputFormatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter(new System.Text.Json.JsonSerializerOptions { }));
                o.OutputFormatters.Add(new Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter());
                o.ReturnHttpNotAcceptable = true;
            });

            services.AddOpenApi();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCustomHeaders();

            app.UseRouting();

            app.UseCorsService();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });

            app.UseOpenApi();
        }


        private readonly IConfiguration configuration;
    }

}
