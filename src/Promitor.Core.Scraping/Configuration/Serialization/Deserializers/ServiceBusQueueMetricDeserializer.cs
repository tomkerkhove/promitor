using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Deserializers
{
    internal class ServiceBusQueueMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>Deserializes the specified Service Bus Queue metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Service Bus queue</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="ServiceBusQueueMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<ServiceBusQueueMetricDefinition>(metricNode);

            var queueName = metricNode.Children[new YamlScalarNode("queueName")];
            var namespaceName = metricNode.Children[new YamlScalarNode("namespace")];

            metricDefinition.QueueName = queueName?.ToString();
            metricDefinition.Namespace = namespaceName?.ToString();

            return metricDefinition;
        }
    }
}
