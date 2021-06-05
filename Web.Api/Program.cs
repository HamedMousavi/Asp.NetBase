using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Web.Api.Logging;


namespace Web.Api
{

    public class Program
    {

        public static int Main(string[] args)
        {
            Logger = new SerilogLogger();

            try
            {
                Logger.Trace("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Logger.Dispose();
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .AddLogger(Logger)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }


        internal static ILogger Logger;
    }
}
