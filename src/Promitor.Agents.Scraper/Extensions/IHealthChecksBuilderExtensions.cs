using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Health;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public static class IHealthChecksBuilderExtensions
    {
        /// <summary>
        ///     Add health check for integration with Promitor Resource Discovery
        /// </summary>
        /// <param name="healthChecksBuilder">Builder for adding health checks</param>
        /// <param name="configuration">Configuration of Promitor</param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddResourceDiscoveryHealthCheck(this IHealthChecksBuilder healthChecksBuilder, IConfiguration configuration)
        {
            Guard.NotNull(healthChecksBuilder, nameof(healthChecksBuilder));
            Guard.NotNull(configuration, nameof(configuration));

            var resourceDiscoveryConfiguration = configuration.GetSection("resourceDiscovery").Get<ResourceDiscoveryConfiguration>();
            if (resourceDiscoveryConfiguration?.IsConfigured == true)
            {
                healthChecksBuilder.AddCheck<ResourceDiscoveryHealthCheck>("Promitor Resource Discovery", HealthStatus.Degraded);
            }

            return healthChecksBuilder;
        }
    }
}