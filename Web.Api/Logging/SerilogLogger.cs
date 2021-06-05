using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Web.Api.Logging
{

    public sealed class SerilogLogger : ILogger
    {

        public SerilogLogger()
        {
            var configFilePath = Path.Combine(StartupPath(), "Logging", "logging.json");
            var config = new ConfigurationBuilder().AddJsonFile(configFilePath).Build();
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
              .ReadFrom.Configuration(config)
              .CreateLogger();
        }


        public void Error(Exception exception, string message)
        {
            Log.Error(exception, message);
        }


        public void Trace(string message)
        {
            Log.Information(message);
        }


        private static string StartupPath()
        {
            return Path.GetDirectoryName(
                System.Net.WebUtility.UrlDecode(new UriBuilder(Assembly.GetExecutingAssembly().Location).Path));
        }


        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }



    public static class SerilogLoggerExtensions
    {
        public static IHostBuilder AddLogger(this IHostBuilder builder, ILogger logger)
        {
            return builder
                .ConfigureLogging(lb =>
                {
                    lb.ClearProviders();
                })
                .UseSerilog()
                .ConfigureServices((hostContext, services) => { services.AddSingleton(logger); });
        }
    }
}
