using System;
using Promitor.Scraper.Configuration;
using Promitor.Scraper.Model;
using Promitor.Scraper.Model.Configuration;
using Promitor.Scraper.Model.Configuration.Metrics;
using Promitor.Scraper.Scraping.Interfaces;
using Promitor.Scraper.Scraping.ResouceTypes;

namespace Promitor.Scraper.Scraping.Factories
{
    internal class MetricScraperFactory
    {
        /// <summary>
        ///     Creates a scraper that is capable of scraping a specific resource type
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="metricDefinitionResourceType">Resource type to scrape</param>
        internal static IScraper<MetricDefinition> CreateScraper(AzureMetadata azureMetadata, ResourceType metricDefinitionResourceType)
        {
            var azureCredentials = DetermineAzureCredentials();

            switch (metricDefinitionResourceType)
            {
                case ResourceType.ServiceBusQueue:
                    return new ServiceBusQueueScraper(azureMetadata, azureCredentials);
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