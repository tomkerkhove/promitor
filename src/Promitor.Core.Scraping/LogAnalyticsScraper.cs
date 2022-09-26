using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Monitor.Query.Models;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Integrations.Azure.Authentication;
using Promitor.Integrations.AzureMonitor.Exceptions;
using Promitor.Integrations.LogAnalytics;

namespace Promitor.Core.Scraping
{
    public class LogAnalyticsScraper : Scraper<LogAnalyticsResourceDefinition>
    {
        private readonly LogAnalyticsClient _logAnalyticsClient;

        public LogAnalyticsScraper(ScraperConfiguration scraperConfiguration) : base(scraperConfiguration)
        {
            _logAnalyticsClient = scraperConfiguration.LogAnalyticsClient;
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, LogAnalyticsResourceDefinition resourceDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            Guard.NotNull(scrapeDefinition, nameof(scrapeDefinition));
            Guard.NotNull(scrapeDefinition.LogAnalyticsConfiguration, nameof(scrapeDefinition.LogAnalyticsConfiguration));

            string resourceUri = null;
            var query = scrapeDefinition.LogAnalyticsConfiguration.Query;
            var workspaceId = resourceDefinition.WorkspaceId;

            List<MeasuredMetric> measuredMetrics = new List<MeasuredMetric>();
            try
            {
                // Query Azure Monitor for metrics
                var result = await _logAnalyticsClient.QueryDouble(workspaceId, query);
                var measuredMetric = MeasuredMetric.CreateWithoutDimension(result);
                measuredMetrics.Add(measuredMetric);
            }
            catch (Exception e)
            {
                Logger.LogWarning("Something wrong when query logs analytics");
                var measuredMetric = MeasuredMetric.CreateWithoutDimension(null);
                measuredMetrics.Add(measuredMetric);
            }

            var metricLabels = DetermineMetricLabels(resourceDefinition);

            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, resourceDefinition.ResourceName, resourceUri, measuredMetrics, metricLabels);
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, LogAnalyticsResourceDefinition resource)
        {
            return null;
        }

        private Dictionary<string, string> DetermineMetricLabels(LogAnalyticsResourceDefinition resourceDefinition)
        {
            return new Dictionary<string, string> { { "workspace_id", resourceDefinition.WorkspaceId } };
        }
    }
}