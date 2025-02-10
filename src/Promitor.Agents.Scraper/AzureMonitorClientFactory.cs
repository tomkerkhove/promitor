using System.Collections.Concurrent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper.Extensions;
using Promitor.Core.Configuration;
using Promitor.Core.Extensions;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Serialization.Enum;
using Promitor.Integrations.Azure.Authentication;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.AzureMonitor.Configuration;

namespace Promitor.Agents.Scraper
{
    public class AzureMonitorClientFactory
    {
        private readonly ConcurrentDictionary<string, IAzureMonitorClient> _azureMonitorClients = new();

        /// <summary>
        /// Provides an Azure Monitor client
        /// </summary>
        /// <param name="azureMetadata">Azure metadata information</param>
        /// <param name="tenantId">Id of the tenant that owns the Azure subscription</param>
        /// <param name="subscriptionId">Id of the Azure subscription</param>
        /// <param name="metricSinkWriter">Writer to send metrics to all configured sinks</param>
        /// <param name="azureScrapingSystemMetricsPublisher">Metrics collector to write metrics to Prometheus</param>
        /// <param name="resourceMetricDefinitionMemoryCache">Memory cache to store items in</param>
        /// <param name="configuration">Configuration of Promitor</param>
        /// <param name="azureMonitorIntegrationConfiguration">Options for Azure Monitor integration</param>
        /// <param name="azureMonitorLoggingConfiguration">Options for Azure Monitor logging</param>
        /// <param name="loggerFactory">Factory to create loggers with</param>
        public IAzureMonitorClient CreateIfNotExists(AzureMetadata azureMetadata, string tenantId, string subscriptionId, MetricSinkWriter metricSinkWriter, IAzureScrapingSystemMetricsPublisher azureScrapingSystemMetricsPublisher, IMemoryCache resourceMetricDefinitionMemoryCache, IConfiguration configuration, IOptions<AzureMonitorIntegrationConfiguration> azureMonitorIntegrationConfiguration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory)
        {
            if (_azureMonitorClients.TryGetValue(subscriptionId, out var value))
            {
                return value;
            }
            var useAzureMonitorSdk = azureMonitorIntegrationConfiguration.Value.UseAzureMonitorSdk;
            IAzureMonitorClient azureMonitorClient;
            if (useAzureMonitorSdk)
            {
                azureMonitorClient = CreateNewAzureMonitorQueryClient(azureMetadata, tenantId, subscriptionId, metricSinkWriter, azureScrapingSystemMetricsPublisher, resourceMetricDefinitionMemoryCache, configuration, azureMonitorIntegrationConfiguration, azureMonitorLoggingConfiguration, loggerFactory);
            }
            else
            {
                azureMonitorClient = CreateNewLegacyAzureMonitorClient(azureMetadata.GetAzureEnvironment(), tenantId, subscriptionId, metricSinkWriter, azureScrapingSystemMetricsPublisher, resourceMetricDefinitionMemoryCache, configuration, azureMonitorIntegrationConfiguration, azureMonitorLoggingConfiguration, loggerFactory);
            }
            _azureMonitorClients.TryAdd(subscriptionId, azureMonitorClient);

            return azureMonitorClient;
        }

        private static LegacyAzureMonitorClient CreateNewLegacyAzureMonitorClient(AzureEnvironment cloud, string tenantId, string subscriptionId, MetricSinkWriter metricSinkWriter, IAzureScrapingSystemMetricsPublisher azureScrapingSystemMetricsPublisher, IMemoryCache resourceMetricDefinitionMemoryCache, IConfiguration configuration, IOptions<AzureMonitorIntegrationConfiguration> azureMonitorIntegrationConfiguration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory)
        {
            var azureCredentials = AzureAuthenticationFactory.GetConfiguredAzureAuthentication(configuration);
            var azureMonitorClientLegacy = new LegacyAzureMonitorClient(cloud, tenantId, subscriptionId, azureCredentials, metricSinkWriter, azureScrapingSystemMetricsPublisher, resourceMetricDefinitionMemoryCache, loggerFactory, azureMonitorIntegrationConfiguration, azureMonitorLoggingConfiguration);
            return azureMonitorClientLegacy;
        }

        private static AzureMonitorQueryClient CreateNewAzureMonitorQueryClient(AzureMetadata azureMetadata, string tenantId, string subscriptionId, MetricSinkWriter metricSinkWriter, IAzureScrapingSystemMetricsPublisher azureScrapingSystemMetricsPublisher, IMemoryCache resourceMetricDefinitionMemoryCache, IConfiguration configuration, IOptions<AzureMonitorIntegrationConfiguration> azureMonitorIntegrationConfiguration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory)
        {
            var azureCredentials = AzureAuthenticationFactory.GetConfiguredAzureAuthentication(configuration);
            var azureMonitorQueryClient = new AzureMonitorQueryClient(azureMetadata, tenantId, subscriptionId, azureCredentials, metricSinkWriter, azureScrapingSystemMetricsPublisher, resourceMetricDefinitionMemoryCache, loggerFactory, azureMonitorIntegrationConfiguration, azureMonitorLoggingConfiguration);
            return azureMonitorQueryClient;
        }
    }
}
