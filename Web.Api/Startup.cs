using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Middlewares;
using Microsoft.Extensions.Configuration;
using Web.Api.Settings;

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
        }


        private readonly IConfiguration configuration;

    }

}
