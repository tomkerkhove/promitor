using System;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Scraping.Interfaces;
using Promitor.Scraper.Host.Scraping.ResouceTypes;

namespace Promitor.Scraper.Host.Scraping.Factories
{
    internal class MetricScraperFactory
    {
        /// <summary>
        ///     Creates a scraper that is capable of scraping a specific resource type
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="metricDefinitionResourceType">Resource type to scrape</param>
        /// <param name="exceptionTracker">Tracker used to log exceptions</param>
        internal static IScraper<MetricDefinition> CreateScraper(AzureMetadata azureMetadata, ResourceType metricDefinitionResourceType, IExceptionTracker exceptionTracker)
        {
            var azureCredentials = DetermineAzureCredentials();

            switch (metricDefinitionResourceType)
            {
                case ResourceType.ServiceBusQueue:
                    return new ServiceBusQueueScraper(azureMetadata, azureCredentials, exceptionTracker);
                default:
                    throw new ArgumentOutOfRangeException();
            }
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