using Microsoft.Azure.Management.Monitor.Models;

namespace Promitor.Scraper.Model.Configuration.Metrics
{
    public class MetricDefinition
    {
        /// <summary>
        ///     Name of the Azure Monitor metric to query
        /// </summary>
        public string AzureMetricName { get; set; }

        /// <summary>
        ///     Type of aggregation to query the Azure Monitor metric
        /// </summary>
        public AggregationType AzureMetricAggregation { get; set; }

        /// <summary>
        ///     Description concerning metric that will be made available in the scraping endpoint
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Name of the metric to use when exposing in the scraping endpoint
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Type of resource that is configured
        /// </summary>
        public virtual ResourceType ResourceType { get; set; }
    }
}