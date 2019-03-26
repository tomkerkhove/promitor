﻿using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Deserializers
{
    internal class StorageQueueMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>Deserializes the specified Storage Queue metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Storage queue configuration</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="StorageQueueMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<StorageQueueMetricDefinition>(metricNode);
            var accountName = metricNode.Children[new YamlScalarNode("accountName")];
            var queueName = metricNode.Children[new YamlScalarNode("queueName")];
            var sasToken = metricNode.Children[new YamlScalarNode("sasToken")];

            metricDefinition.AccountName = accountName?.ToString();
            metricDefinition.QueueName = queueName?.ToString();
            metricDefinition.SasToken = sasToken?.ToString();

            return metricDefinition;
        }
    }
}
