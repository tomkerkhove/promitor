using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Scraper.Host.Configuration.Model
{
    public class AzureMetricConfiguration
    {
        /// <summary>
        ///     The particular metric type that azure monitor exposes the metric as
        /// </summary>
        public MetricType MetricType { get; set; }

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