using System.Collections.Generic;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfigurationV1>
    {
        private const string DimensionsTag = "dimensions";
        private const string DimensionTag = "dimension";
        private readonly IDeserializer<MetricDimensionV1> _dimensionDeserializer;
            
        public AzureMetricConfigurationDeserializer(IDeserializer<MetricDimensionV1> dimensionDeserializer, IDeserializer<MetricAggregationV1> aggregationDeserializer, ILogger<AzureMetricConfigurationDeserializer> logger)
            : base(logger)
        {
            Map(config => config.MetricName)
                .IsRequired();
            Map(config => config.Limit)
                .MapUsing(DetermineLimit);
            Map(config => config.Dimension)
                .MapUsingDeserializer(dimensionDeserializer);
            Map(config => config.Aggregation)
                .IsRequired()
                .MapUsingDeserializer(aggregationDeserializer);

            _dimensionDeserializer = dimensionDeserializer;
        }

        public override AzureMetricConfigurationV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var azureMetricConfiguration = base.Deserialize(node, errorReporter);

            // TODO: add backwards compatibility, only accept one of dimension or dimensions and map dimension to list with one entry

            if (node.Children.TryGetValue(DimensionsTag, out var dimensionsNode))
            {
                azureMetricConfiguration.Dimensions = _dimensionDeserializer.Deserialize((YamlSequenceNode)dimensionsNode, errorReporter);
            }

            return azureMetricConfiguration;
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
