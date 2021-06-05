using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Web.Api.Logging;


namespace Web.Api.HealthChecks
{

    public class LoggerHealthCheck : IHealthCheck
    {

        public static string Name => "Logger Health Check";
        public static IEnumerable<string> Tags => new string[] { Name };
        public static HealthStatus Severity => HealthStatus.Unhealthy;


        public LoggerHealthCheck(ILogger logger)
        {
            this.logger = logger;
        }


        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.Trace("Checking logger health...");
            }
            catch (Exception exception)
            {
                try
                {
                    return Task.FromResult(new HealthCheckResult(status: context.Registration.FailureStatus, exception: exception));
                }
                catch
                { /* logger is down and the attempt to report health issues failed, what now?! */ }
            }
            return Task.FromResult(HealthCheckResult.Healthy());
        }


        private readonly ILogger logger;
    }
}
