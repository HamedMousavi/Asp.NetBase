using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Middlewares
{

    public static class CorsExtensions
    {

        public static IServiceCollection AddCorsService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            return services.AddCors(options =>
            {
                options.AddPolicy(name: CorsSettings.PolocyName, builder => {
                    builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(IsOriginAllowed);
                });
            });
        }


        public static IApplicationBuilder UseCorsService(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            corsSettings = app.ApplicationServices.GetService<IOptionsMonitor<CorsSettings>>();
            return app.UseCors(CorsSettings.PolocyName);
        }


        private static bool IsOriginAllowed(string origin)
        {
            if (string.IsNullOrWhiteSpace(origin)) return false;
            return corsSettings.CurrentValue.WhiteList.Any(allowed => 
                string.Equals(origin, allowed, StringComparison.InvariantCultureIgnoreCase));
        }


        private static IOptionsMonitor<CorsSettings> corsSettings;
    }


    public class CorsSettings
    {
        public static string ConfigFileSectionName => "Cors";
        public static string PolocyName => "CorsWhiteList";
        public List<string> WhiteList { get; set; }
    }
}
