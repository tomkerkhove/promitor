using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class ServiceBusTopicMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>Deserializes the specified Service Bus Topic metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Service Bus queue</param>
        /// <returns>
        ///     A new <see cref="MetricDefinition" /> object (strongly typed as a
        ///     <see cref="ServiceBusTopicMetricDefinition" />)
        /// </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<ServiceBusTopicMetricDefinition>(metricNode);

            var namespaceName = metricNode.Children[new YamlScalarNode("namespace")];
            var topicName = metricNode.Children[new YamlScalarNode("topicName")];
            var subscriptionName = metricNode.Children[new YamlScalarNode("subscriptionName")];

            metricDefinition.Namespace = namespaceName?.ToString();
            metricDefinition.TopicName = topicName?.ToString();
            metricDefinition.SubscriptionName = subscriptionName?.ToString();

            return metricDefinition;
        }
    }
}