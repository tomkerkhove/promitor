using System;
using System.Threading;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Promitor.Agents.Scraper.Discovery;

namespace Promitor.Agents.Scraper.Health
{
    public class ResourceDiscoveryHealthCheck : IHealthCheck
    {
        private readonly ResourceDiscoveryClient _resourceDiscoveryClient;

        public ResourceDiscoveryHealthCheck(ResourceDiscoveryClient resourceDiscoveryClient)
        {
            Guard.NotNull(resourceDiscoveryClient, nameof(resourceDiscoveryClient));

            _resourceDiscoveryClient = resourceDiscoveryClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var resourceDiscoveryHealthReport = await _resourceDiscoveryClient.GetHealthAsync();

                if (resourceDiscoveryHealthReport.Status == HealthStatus.Healthy)
                {
                    return HealthCheckResult.Healthy("Successfully contacted Promitor Resource Discovery");
                }

                return HealthCheckResult.Degraded("Successfully contacted Promitor Resource Discovery but it's not in healthy state.");
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Unhealthy("Unable to contacted Promitor Resource Discovery.", exception: exception);
            }
        }
    }
}