using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using Web.Api.Logging;


namespace Web.Api
{

    public class Program
    {

        public static int Main(string[] args)
        {
            log = new SerilogLogger();

            try
            {
                log.Trace("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                log.Error(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                log.Dispose();
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .AddLogger(log)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>();
                });
        }


        private static ILogger log;
    }
}
