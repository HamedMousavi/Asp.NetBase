using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;


namespace Web.Api
{

    public class Startup
    {


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(100);
                options.IncludeSubDomains = true;
                options.Preload = true;
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHsts();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }

}
