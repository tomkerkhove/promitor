﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using Newtonsoft.Json;
using Promitor.Scraper.Model;
using Promitor.Scraper.Model.Configuration;
using Promitor.Scraper.Model.Configuration.Metrics;
using Promitor.Scraper.Scraping.Interfaces;

namespace Promitor.Scraper.Scraping
{
    /// <summary>
    ///     Azure Monitor Scrape
    /// </summary>
    /// <typeparam name="TMetricDefinition">Type of metric definition that is being used</typeparam>
    public abstract class Scraper<TMetricDefinition> : IScraper<MetricDefinition> where TMetricDefinition : MetricDefinition, new()
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="azureCredentials">Credentials used to authenticate to Microsoft Azure</param>
        protected Scraper(AzureMetadata azureMetadata, AzureCredentials azureCredentials)
        {
            AzureMetadata = azureMetadata ?? throw new ArgumentNullException(nameof(azureMetadata));
            AzureCredentials = azureCredentials ?? throw new ArgumentNullException(nameof(azureCredentials));
        }

        /// <summary>
        ///     Credentials used to authenticate to Microsoft Azure
        /// </summary>
        protected AzureCredentials AzureCredentials { get; }

        /// <summary>
        ///     Metadata concerning the Azure resources
        /// </summary>
        protected AzureMetadata AzureMetadata { get; }

        public async Task ScrapeAsync(MetricDefinition metricDefinition)
        {
            if (metricDefinition == null)
            {
                throw new ArgumentNullException(nameof(metricDefinition));
            }

            if (!(metricDefinition is TMetricDefinition castedMetricDefinition))
            {
                throw new ArgumentException($"Could not cast metric definition of type '{metricDefinition.ResourceType}' to {typeof(TMetricDefinition)}. Payload: {JsonConvert.SerializeObject(metricDefinition)}");
            }

            var monitoringClient = await GetAzureMonitorClientAsync();

            await ScrapeResourceAsync(monitoringClient, castedMetricDefinition);
        }

        /// <summary>
        ///     Scrapes the configured resource
        /// </summary>
        /// <param name="monitoringClient">Client to query Azure Monitor</param>
        /// <param name="metricDefinition">Definition of the metric to scrape</param>
        protected abstract Task ScrapeResourceAsync(MonitorManagementClient monitoringClient, TMetricDefinition metricDefinition);

        private async Task<ServiceClientCredentials> AuthenticateWithAzureAsync()
        {
            return await ApplicationTokenProvider.LoginSilentAsync(AzureMetadata.TenantId, AzureCredentials.ApplicationId, AzureCredentials.Secret);
        }

        private async Task<MonitorManagementClient> GetAzureMonitorClientAsync()
        {
            var azureServiceCredentials = await AuthenticateWithAzureAsync();
            var monitoringClient = new MonitorManagementClient(azureServiceCredentials);
            return monitoringClient;
        }
    }
}