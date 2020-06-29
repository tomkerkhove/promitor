using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfigurationV1>
    {
        public AzureMetricConfigurationDeserializer(IDeserializer<MetricDimensionV1> dimensionDeserializer, IDeserializer<MetricAggregationV1> aggregationDeserializer, ILogger<AzureMetricConfigurationDeserializer> logger)
            : base(logger)
        {
            Map(config => config.MetricName)
                .IsRequired();
            Map(config => config.Dimension)
                .MapUsingDeserializer(dimensionDeserializer);
            Map(config => config.Aggregation)
                .IsRequired()
                .MapUsingDeserializer(aggregationDeserializer);
        }
    }
}
