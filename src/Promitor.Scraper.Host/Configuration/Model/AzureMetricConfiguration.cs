using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Scraper.Host.Configuration.Model
{
    public class AzureMetricConfiguration
    {
        /// <summary>
        ///     Type of aggregation to query the Azure Monitor metric
        /// </summary>
        public AggregationType Aggregation { get; set; }

        /// <summary>
        ///     Name of the Azure Monitor metric to query
        /// </summary>
        public string MetricName { get; set; }
    }
}