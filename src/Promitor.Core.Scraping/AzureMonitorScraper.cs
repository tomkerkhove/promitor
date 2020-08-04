using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Contracts;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using MetricDimension = Promitor.Core.Scraping.Configuration.Model.MetricDimension;

namespace Promitor.Core.Scraping
{
    /// <summary>
    ///     Azure Monitor Scraper
    /// </summary>
    /// <typeparam name="TResourceDefinition">Type of metric definition that is being used</typeparam>
    public abstract class AzureMonitorScraper<TResourceDefinition> : Scraper<TResourceDefinition>
        where TResourceDefinition : class, IAzureResourceDefinition
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        protected AzureMonitorScraper(ScraperConfiguration scraperConfiguration) :
            base(scraperConfiguration)
        {
        }

        /// <inheritdoc />
        protected override async Task<ScrapeResult> ScrapeResourceAsync(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, TResourceDefinition resourceDefinition, AggregationType aggregationType, TimeSpan aggregationInterval)
        {
            var resourceUri = BuildResourceUri(subscriptionId, scrapeDefinition, resourceDefinition);

            var metricFilter = DetermineMetricFilter(resourceDefinition);
            var metricName = scrapeDefinition.AzureMetricConfiguration.MetricName;
            var dimensionName = DetermineMetricDimension(scrapeDefinition.AzureMetricConfiguration?.Dimension);
            var foundMetricValue = await AzureMonitorClient.QueryMetricAsync(metricName, dimensionName, aggregationType, aggregationInterval, resourceUri, metricFilter);

            var metricLabels = DetermineMetricLabels(resourceDefinition);
            
            return new ScrapeResult(subscriptionId, scrapeDefinition.ResourceGroupName, resourceDefinition.ResourceName, resourceUri, foundMetricValue, metricLabels);
        }

        /// <summary>
        ///     Determines the metric filter to use
        /// </summary>
        /// <param name="resourceDefinition">Contains the resource cast to the specific resource type.</param>
        protected virtual string DetermineMetricFilter(TResourceDefinition resourceDefinition)
        {
            return null;
        }

        /// <summary>
        ///     Determines the dimension for a metric to use
        /// </summary>
        /// <param name="dimension">Provides information concerning the configured metric dimension.</param>
        protected virtual string DetermineMetricDimension(MetricDimension dimension)
        {
            return dimension?.Name;
        }

        /// <summary>
        ///     Determines the metric labels to include in the reported metric
        /// </summary>
        /// <param name="resourceDefinition">Contains the resource cast to the specific resource type.</param>
        protected virtual Dictionary<string, string> DetermineMetricLabels(TResourceDefinition resourceDefinition)
        {
            return new Dictionary<string, string>();
        }
    }
}