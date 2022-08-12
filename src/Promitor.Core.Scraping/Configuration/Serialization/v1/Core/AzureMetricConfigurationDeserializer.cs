using System.Collections.Generic;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfigurationV1>
    {
        private const string MultipleDimensionsTag = "dimensions";
        private const string SingleDimensionTag = "dimension";
        private readonly IDeserializer<MetricDimensionV1> _dimensionDeserializer;
            
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
            IgnoreField(MultipleDimensionsTag);
            IgnoreField(SingleDimensionTag);

            _dimensionDeserializer = dimensionDeserializer;
        }

        public override AzureMetricConfigurationV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var azureMetricConfiguration = base.Deserialize(node, errorReporter);

            if (node.Children.TryGetValue(MultipleDimensionsTag, out var multipleDimensionsNode))
            {
                azureMetricConfiguration.Dimensions = _dimensionDeserializer.Deserialize((YamlSequenceNode)multipleDimensionsNode, errorReporter);
            }
            
            else if (node.Children.TryGetValue(SingleDimensionTag, out var singleDimensionNode))
            {
                errorReporter.ReportWarning(node, "Usage of 'dimension' is deprecated in favor of using 'dimensions'.");
                azureMetricConfiguration.Dimensions = new List<MetricDimensionV1>{ _dimensionDeserializer.Deserialize((YamlMappingNode)singleDimensionNode, errorReporter) };
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
