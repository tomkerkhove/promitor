using Microsoft.Azure.Management.Monitor.Models;

namespace Promitor.Scraper.Model.Configuration
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