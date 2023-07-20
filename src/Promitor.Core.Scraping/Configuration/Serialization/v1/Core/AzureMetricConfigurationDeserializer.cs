using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfigurationV1>
    {
        public AzureMetricConfigurationDeserializer(IDeserializer<MetricDimensionV1> dimensionDeserializer, IDeserializer<MetricAggregationV1> aggregationDeserializer, ILogger<AzureMetricConfigurationDeserializer> logger)
            : base(logger)
        {
            Map(config => config.MetricName)
                .IsRequired();
            Map(config => config.Limit)
                .MapUsing(DetermineLimit);
            Map(config => config.Aggregation)
                .IsRequired()
                .MapUsingDeserializer(aggregationDeserializer);
            Map(config => config.Dimensions)
                .MapUsingDeserializer(dimensionDeserializer);
            Map(config => config.Dimension)
                .MapUsingDeserializer(dimensionDeserializer);
        }

        private object DetermineLimit(string rawLimit, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
        {
            if (int.TryParse(rawLimit, out int limit))
            {
                return limit;
            }

            return null;
        }
    }
}
