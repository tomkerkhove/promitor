﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping.ResourceTypes
{
    internal class GenericScraper : Scraper<GenericAzureMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/{2}";

        public GenericScraper(AzureMetadata azureMetadata, MetricDefaults metricDefaults, AzureMonitorClient azureMonitorClient, ILogger logger, IExceptionTracker exceptionTracker)
            : base(azureMetadata, metricDefaults, azureMonitorClient, logger, exceptionTracker)
        {
        }

        protected override async Task<double> ScrapeResourceAsync(GenericAzureMetricDefinition metricDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, AzureMetadata.ResourceGroupName, metricDefinition.ResourceUri);
            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, aggregationType, aggregationInterval, resourceUri, metricDefinition.Filter);

            return foundMetricValue;
        }
    }
}