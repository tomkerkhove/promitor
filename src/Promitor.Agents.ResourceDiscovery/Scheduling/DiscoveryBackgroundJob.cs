using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Core.Metrics.Interfaces;

namespace Promitor.Agents.ResourceDiscovery.Scheduling
{
    public class DiscoveryBackgroundJob
    {
        protected IAzureResourceRepository AzureResourceRepository { get; }
        protected ILogger Logger { get; }
        
        private readonly ISystemMetricsPublisher _systemMetricsPublisher;

        public DiscoveryBackgroundJob(IAzureResourceRepository azureResourceRepository, ISystemMetricsPublisher systemMetricsPublisher, ILogger logger)
        {
            Guard.NotNull(systemMetricsPublisher, nameof(systemMetricsPublisher));
            Guard.NotNull(azureResourceRepository, nameof(azureResourceRepository));

            Logger = logger;
            _systemMetricsPublisher = systemMetricsPublisher;
            AzureResourceRepository = azureResourceRepository;
        }

        protected async Task WritePrometheusMetricAsync(string metricName, string metricDescription, int value, Dictionary<string, string> labels)
        {
            await _systemMetricsPublisher.WriteGaugeMeasurementAsync(metricName, metricDescription, value, labels, includeTimestamp: true);
        }

        protected string GetValueOrDefault(string preferredValue, string alternative)
        {
            if (string.IsNullOrWhiteSpace(preferredValue))
            {
                return alternative;
            }

            return preferredValue;
        }
    }
}
