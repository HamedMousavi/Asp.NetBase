using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;


namespace Web.Api.Middlewares
{

    public static class OpenApiExtensions
    {

        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            return services.AddSwaggerGen(c =>
            {
                // "/swagger/v1/swagger.json"
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "<<My Cool API Title>>", // todo
                    Description = "<<Description of the api!>>", //todo
                    TermsOfService = new Uri("https://solidsoft.ir/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Hamed Musave",
                        Email = string.Empty,
                        Url = new Uri("https://linkedin.com/hamedmusavi"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under Apache",
                        Url = new Uri("https://solidsoft.ir/license"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // This section can be used to describe potential authentication requirements
                c.AddSecurityDefinition("Authentication", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OpenIdConnect,
                    Scheme = "Bearer Token",
                    Description = "This api uses OpenIdConnect for authentication"
                });
                // To enforce authentication for each api, uncomment this section.
                //c.AddSecurityRequirement();
            });
        }


        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.RoutePrefix = string.Empty;
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "<<API V1>>");
            });
            return app;
        }
    }
}
