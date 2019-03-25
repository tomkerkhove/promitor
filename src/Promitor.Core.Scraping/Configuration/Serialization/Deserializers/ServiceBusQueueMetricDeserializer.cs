using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Deserializers
{
    internal class ServiceBusQueueMetricDeserializer : GenericAzureMetricDeserializer
    {
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.Deserialize<ServiceBusQueueMetricDefinition>(metricNode);

            var queueName = metricNode.Children[new YamlScalarNode("queueName")];
            var namespaceName = metricNode.Children[new YamlScalarNode("namespace")];

            metricDefinition.QueueName = queueName?.ToString();
            metricDefinition.Namespace = namespaceName?.ToString();

            return metricDefinition;
        }
    }
}
