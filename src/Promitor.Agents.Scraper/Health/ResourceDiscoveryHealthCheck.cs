using System;
using System.Threading;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Promitor.Agents.Scraper.Discovery.Interfaces;

namespace Promitor.Agents.Scraper.Health
{
    public class ResourceDiscoveryHealthCheck : IHealthCheck
    {
        private readonly IResourceDiscoveryRepository _resourceDiscoveryRepository;

        public ResourceDiscoveryHealthCheck(IResourceDiscoveryRepository resourceDiscoveryRepository)
        {
            Guard.NotNull(resourceDiscoveryRepository, nameof(resourceDiscoveryRepository));

            _resourceDiscoveryRepository = resourceDiscoveryRepository;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            try
            {
                var resourceDiscoveryHealthReport = await _resourceDiscoveryRepository.GetHealthAsync();
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