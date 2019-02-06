using System;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResouceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    internal class MetricsDeserializer : Deserializer<MetricDefinition>
    {
        internal MetricsDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override MetricDefinition Deserialize(YamlMappingNode node)
        {
            var rawResourceType = node.Children[new YamlScalarNode("resourceType")];
            var resourceType = Enum.Parse<ResourceType>(rawResourceType.ToString());

            MetricDefinition metricDefinition;
            switch (resourceType)
            {
                case ResourceType.ServiceBusQueue:
                    metricDefinition = DeserializeServiceBusQueueMetric(node);
                    break;
                case ResourceType.Generic:
                    metricDefinition = DeserializeGenericMetric(node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, $"No deserialization was found for {resourceType}");
            }

            return metricDefinition;
        }

        private MetricDefinition DeserializeGenericMetric(YamlMappingNode metricNode)
        {
            var metricDefinition = DeserializeMetricDefinition<GenericMetricDefinition>(metricNode);

            if(metricNode.Children.TryGetValue(new YamlScalarNode("filter"), out var filterNode))
            {
                metricDefinition.Filter = filterNode?.ToString();
            }

            var resourceUri = metricNode.Children[new YamlScalarNode("resourceUri")];

            metricDefinition.ResourceUri = resourceUri?.ToString();

            return metricDefinition;
        }

        private MetricDefinition DeserializeServiceBusQueueMetric(YamlMappingNode metricNode)
        {
            var metricDefinition = DeserializeMetricDefinition<ServiceBusQueueMetricDefinition>(metricNode);

            var queueName = metricNode.Children[new YamlScalarNode("queueName")];
            var namespaceName = metricNode.Children[new YamlScalarNode("namespace")];

            metricDefinition.QueueName = queueName?.ToString();
            metricDefinition.Namespace = namespaceName?.ToString();

            return metricDefinition;
        }

        private TMetricDefinition DeserializeMetricDefinition<TMetricDefinition>(YamlMappingNode metricNode)
            where TMetricDefinition : MetricDefinition, new()
        {
            Guard.NotNull(metricNode, nameof(metricNode));

            var name = metricNode.Children[new YamlScalarNode("name")];
            var description = metricNode.Children[new YamlScalarNode("description")];
            var azureMetricConfigurationNode = (YamlMappingNode) metricNode.Children[new YamlScalarNode("azureMetricConfiguration")];

            var azureMetricConfigurationDeserializer = new AzureMetricConfigurationDeserializer(Logger);
            var azureMetricConfiguration = azureMetricConfigurationDeserializer.Deserialize(azureMetricConfigurationNode);

            var metricDefinition = new TMetricDefinition
            {
                Name = name?.ToString(),
                Description = description?.ToString(),
                AzureMetricConfiguration = azureMetricConfiguration
            };

            return metricDefinition;
        }
    }
}