using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfigurationV1>
    {
        public AzureMetricConfigurationDeserializer(IDeserializer<MetricDimensionV1> dimensionDeserializer, IDeserializer<MetricAggregationV1> aggregationDeserializer, ILogger<AzureMetricConfigurationDeserializer> logger)
            : base(logger)
        {
            MapRequired(config => config.MetricName);
            MapOptional(config => config.Dimension, dimensionDeserializer);
            MapRequired(config => config.Aggregation, aggregationDeserializer);
        }
    }
}
