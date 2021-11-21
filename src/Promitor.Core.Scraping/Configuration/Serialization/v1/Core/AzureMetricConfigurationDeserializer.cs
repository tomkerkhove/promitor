using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureMetricConfigurationDeserializer : Deserializer<AzureMetricConfigurationV1>
    {
        public AzureMetricConfigurationDeserializer(IDeserializer<MetricInformationV1> metricInformationDeserializer, IDeserializer<MetricDimensionV1> dimensionDeserializer, IDeserializer<MetricAggregationV1> aggregationDeserializer, ILogger<AzureMetricConfigurationDeserializer> logger)
            : base(logger)
        {
            Map(config => config.MetricName);
            Map(config => config.Metric)
                .MapUsingDeserializer(metricInformationDeserializer);
            Map(config => config.Limit)
                .MapUsing(DetermineLimit);
            Map(config => config.Dimension)
                .MapUsingDeserializer(dimensionDeserializer);
            Map(config => config.Aggregation)
                .IsRequired()
                .MapUsingDeserializer(aggregationDeserializer);
        }

        private object DetermineLimit(string rawLimit, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
        {
            if (int.TryParse(rawLimit, out int limit))
            {
                return limit;
            }

            return null;
        }

        public override AzureMetricConfigurationV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var metricConfiguration = base.Deserialize(node, errorReporter);

            if (string.IsNullOrEmpty(metricConfiguration.MetricName) && string.IsNullOrEmpty(metricConfiguration.Metric?.Name))
            {
                errorReporter.ReportError(node, "No metric name was configured. Either 'metricName' or 'metric.name' must be specified.");
            }

            return metricConfiguration;
        }
    }
}
