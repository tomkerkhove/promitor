using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricAggregation : Aggregation
    {
        /// <summary>
        ///     Type of aggregation to query the Azure Monitor metric
        /// </summary>
        public AggregationType Type { get; set; }
    }
}