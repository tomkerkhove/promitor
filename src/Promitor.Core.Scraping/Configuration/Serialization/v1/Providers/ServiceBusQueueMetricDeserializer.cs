using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class ServiceBusQueueMetricDeserializer : MetricDeserializer
    {
        /// <summary>Deserializes the specified Service Bus Queue metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Service Bus queue</param>
        /// <returns>A new <see cref="MetricDefinitionBuilder"/> object (strongly typed as a <see cref="ServiceBusQueueMetricDefinitionBuilder"/>) </returns>
        internal override MetricDefinitionBuilder Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<ServiceBusQueueMetricDefinitionBuilder>(metricNode);

            var queueName = metricNode.Children[new YamlScalarNode("queueName")];
            var namespaceName = metricNode.Children[new YamlScalarNode("namespace")];

            metricDefinition.QueueName = queueName?.ToString();
            metricDefinition.Namespace = namespaceName?.ToString();

            return metricDefinition;
        }
    }
}
