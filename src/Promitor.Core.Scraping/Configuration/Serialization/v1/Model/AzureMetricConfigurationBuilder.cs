using Promitor.Core.Scraping.Configuration.Model;
using YamlDotNet.Serialization;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AzureMetricConfigurationBuilder
    {
        /// <summary>
        ///     Name of the Azure Monitor metric to query
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        ///     Configuration on how to aggregate the metric
        /// </summary>
        [YamlMember(Alias = "aggregation")]
        public MetricAggregationBuilder AggregationBuilder { get; set; }

        public AzureMetricConfiguration Build()
        {
            return new AzureMetricConfiguration
            {
                MetricName = MetricName,
                Aggregation = AggregationBuilder.Build()
            };
        }
    }
}