using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Middlewares
{

    public class CustomHeadersMiddleware
    {

        public CustomHeadersMiddleware(RequestDelegate next, CustomHeadersSettings settings)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.settings = settings;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            context.Response.OnStarting(OnResponseStarting, Tuple.Create(context, settings));
            await next(context);
        }


        private Task OnResponseStarting(object arg)
        {
            var tuple = (Tuple<HttpContext, CustomHeadersSettings>)arg;
            var context = tuple.Item1;
            var settings = tuple.Item2;
            foreach (var headerValuePair in settings)
            {
                context.Response.Headers[headerValuePair.Key] = headerValuePair.Value;
            }
            return Task.CompletedTask;
        }


        private readonly RequestDelegate next;
        private readonly CustomHeadersSettings settings;
    }


    public static class CustomHeadersExtensions
    {
        public static IApplicationBuilder UseCustomHeaders(this IApplicationBuilder app, CustomHeadersSettings headers)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            return app.UseMiddleware<CustomHeadersMiddleware>(headers);
        }
    }

    public class CustomHeadersSettings : Dictionary<string, string>
    {
        public static string ConfigFileSectionName => "CustomHeaders";
    }
}
