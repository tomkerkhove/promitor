using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Integrations.LogAnalytics;

namespace Promitor.Core.Scraping
{
    public class LogAnalyticsScraper : Scraper<LogAnalyticsResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.OperationalInsights/workspaces/{2}/Overview";
        private readonly LogAnalyticsClient _logAnalyticsClient;

        public LogAnalyticsScraper(ScraperConfiguration scraperConfiguration) : base(scraperConfiguration)
        {
            _logAnalyticsClient = scraperConfiguration.LogAnalyticsClient;
        }

        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, LogAnalyticsResourceDefinition resourceDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            Guard.NotNull(scrapeDefinition, nameof(scrapeDefinition));
            Guard.NotNull(scrapeDefinition.LogAnalyticsConfiguration, nameof(scrapeDefinition.LogAnalyticsConfiguration));

            string resourceUri = BuildResourceUri(subscriptionId, scrapeDefinition, resourceDefinition);
            var query = scrapeDefinition.LogAnalyticsConfiguration.Query;
            var workspaceId = resourceDefinition.WorkspaceId;

            List<MeasuredMetric> measuredMetrics = new List<MeasuredMetric>();

            // Query Azure Log Analytics for result
            var result = await _logAnalyticsClient.RunKustoQueryAsync(workspaceId, query, aggregationInterval);
            var measuredMetric = MeasuredMetric.CreateWithoutDimensions(result);
            measuredMetrics.Add(measuredMetric);
            var metricLabels = DetermineMetricLabels(resourceDefinition);

            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, resourceDefinition.ResourceName, resourceUri, measuredMetrics, metricLabels);
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, LogAnalyticsResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, resource.ResourceGroupName, resource.WorkspaceName);
        }

        private Dictionary<string, string> DetermineMetricLabels(LogAnalyticsResourceDefinition resourceDefinition)
        {
            return new Dictionary<string, string> { { "workspace_id", resourceDefinition.WorkspaceId }, {"workspace_name", resourceDefinition.WorkspaceName} };
        }
    }
}