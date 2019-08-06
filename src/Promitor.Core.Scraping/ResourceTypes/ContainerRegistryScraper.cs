﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Prometheus.Interfaces;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class ContainerRegistryScraper : Scraper<ContainerRegistryMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ContainerRegistry/registries/{2}";

        public ContainerRegistryScraper(ScraperConfiguration scraperConfiguration, IPrometheusMetricWriter prometheusMetricWriter)
            : base(scraperConfiguration, prometheusMetricWriter)
        {
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, string resourceGroupName, ContainerRegistryMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, subscriptionId, resourceGroupName, metricDefinition.RegistryName);

            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri);

            return new ScrapeResult(subscriptionId, resourceGroupName, metricDefinition.RegistryName, resourceUri, foundMetricValue);
        }
    }
}