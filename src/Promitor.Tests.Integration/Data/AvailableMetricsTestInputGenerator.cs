using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Infrastructure;

namespace Promitor.Tests.Integration.Data
{
    public class AvailableMetricsTestInputGenerator : IEnumerable<object[]>
    {
        private readonly ResourceDiscoveryClient _resourceDiscoveryClient;
        private readonly ScraperClient _scraperClient;

        public AvailableMetricsTestInputGenerator()
        {
            var logger = NullLogger<AvailableMetricsTestInputGenerator>.Instance;
            var configuration = ConfigurationFactory.Create();

            _scraperClient = new ScraperClient(configuration, logger);
            _resourceDiscoveryClient = new ResourceDiscoveryClient(configuration, logger);
        }

        IEnumerator<object[]> IEnumerable<object[]>.GetEnumerator()
        {
            var configuredMetrics = GetConfiguredMetrics();

            // We need to filter out configured metrics that only have resource discovery groups, but don't have any matching resources
            // Otherwise, we'll expect metrics to show up while they will never be reported which is by design.
            var foundMetricNames = FilterResourceGroupsWithoutResources(configuredMetrics);

            return foundMetricNames.Select(x => new object[] { x }).GetEnumerator();
        }

        private List<string> FilterResourceGroupsWithoutResources(List<MetricDefinition> configuredMetrics)
        {
            List<string> foundMetricNames = new List<string>();
            foreach (var configuredMetric in configuredMetrics)
            {
                bool resourceFound = true;
                if (configuredMetric.Resources?.Any() == false)
                {
                    foreach (var discoveryGroup in configuredMetric.ResourceDiscoveryGroups)
                    {
                        var discoveredResources = _resourceDiscoveryClient.GetAllDiscoveredResourcesAsync(discoveryGroup.Name).Result;
                        if (discoveredResources?.Any() != true)
                        {
                            resourceFound = false;
                            break;
                        }
                    }
                }

                if (resourceFound)
                {
                    foundMetricNames.Add(configuredMetric.PrometheusMetricDefinition.Name);
                }
            }

            return foundMetricNames;
        }

        private List<MetricDefinition> GetConfiguredMetrics()
        {
            return _scraperClient.GetMetricDeclarationAsync().Result;
        }

        // ReSharper disable once FunctionRecursiveOnAllPaths
        public IEnumerator GetEnumerator() => GetEnumerator();
    }
}
