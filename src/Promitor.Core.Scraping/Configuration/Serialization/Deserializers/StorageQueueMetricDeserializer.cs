using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Deserializers
{
    internal class StorageQueueMetricDeserializer : GenericAzureMetricDeserializer
    {
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.Deserialize<StorageQueueMetricDefinition>(metricNode);
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
