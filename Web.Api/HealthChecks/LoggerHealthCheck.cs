using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Api.HealthChecks
{
    public class LoggerHealthCheck : IHealthCheck
    {
        public static string Name => "Logger Health Check";
        public static IEnumerable<string> Tags => new string[] { Name };
        public static HealthStatus Severity => HealthStatus.Unhealthy;

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                //throw new InvalidOperationException("An exception produced at test time.");
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
    }
}
