using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Interfaces;
using Promitor.Core.Scraping.ResourceTypes;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping.Factories
{
    public class MetricScraperFactory
    {
        /// <summary>
        ///     Creates a scraper that is capable of scraping a specific resource type
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="metricDefinitionResourceType">Resource type to scrape</param>
        /// <param name="metricDefaults">Default configuration for metrics</param>
        /// <param name="logger">General logger</param>
        /// <param name="exceptionTracker">Tracker used to log exceptions</param>
        public static IScraper<MetricDefinition> CreateScraper(ResourceType metricDefinitionResourceType, AzureMetadata azureMetadata,
            ILogger logger, IExceptionTracker exceptionTracker)
        {
            var azureCredentials = DetermineAzureCredentials();
            var azureMonitorClient = CreateAzureMonitorClient(azureMetadata, azureCredentials, logger);

            switch (metricDefinitionResourceType)
            {
                case ResourceType.ServiceBusQueue:
                    return new ServiceBusQueueScraper(azureMetadata, azureMonitorClient, logger, exceptionTracker);
                case ResourceType.Generic:
                    return new GenericScraper(azureMetadata, azureMonitorClient, logger, exceptionTracker);
                case ResourceType.StorageQueue:
                    return new StorageQueueScraper(azureMetadata, azureMonitorClient, logger, exceptionTracker);
                case ResourceType.ContainerInstance:
                    return new ContainerInstanceScraper(azureMetadata, azureMonitorClient, logger, exceptionTracker);
                case ResourceType.VirtualMachine:
                    return new VirtualMachineScraper(azureMetadata, azureMonitorClient, logger, exceptionTracker);
                case ResourceType.NetworkInterface:
                    return new NetworkInterfaceScraper(azureMetadata, azureMonitorClient, logger, exceptionTracker);
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryScraper(azureMetadata, azureMonitorClient, logger, exceptionTracker);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static AzureMonitorClient CreateAzureMonitorClient(AzureMetadata azureMetadata, AzureCredentials azureCredentials, ILogger logger)
        {
            var azureMonitorClient = new AzureMonitorClient(azureMetadata.TenantId, azureMetadata.SubscriptionId, azureCredentials.ApplicationId, azureCredentials.Secret, logger);
            return azureMonitorClient;
        }

        private static AzureCredentials DetermineAzureCredentials()
        {
            var applicationId = Environment.GetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationId);
            var applicationKey = Environment.GetEnvironmentVariable(EnvironmentVariables.Authentication.ApplicationKey);

            return new AzureCredentials
            {
                ApplicationId = applicationId,
                Secret = applicationKey
            };
        }
    }
}