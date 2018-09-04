using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Scraper.Host.Serialization;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using MetricDefinition = Promitor.Scraper.Host.Configuration.Model.Metrics.MetricDefinition;

namespace Promitor.Scraper.Host.Configuration.Serialization
{
    public class ConfigurationSerializer
    {
        public static MetricsDeclaration Deserialize(string rawMetricsDeclaration)
        {
            Guard.Guard.NotNullOrWhitespace(rawMetricsDeclaration, nameof(rawMetricsDeclaration));
            var input = new StringReader(rawMetricsDeclaration);
            var metricsDeclarationYamlStream = new YamlStream();
            metricsDeclarationYamlStream.Load(input);

            var metricsDeclaration = InterpretYamlStream(metricsDeclarationYamlStream);

            return metricsDeclaration;
        }

        private static MetricsDeclaration InterpretYamlStream(YamlStream metricsDeclarationYamlStream)
        {
            var document = metricsDeclarationYamlStream.Documents.First();
            var rootNode = (YamlMappingNode)document.RootNode;

            AzureMetadata azureMetadata = null;
            if (rootNode.Children.ContainsKey("azureMetadata"))
            {
                // TODO: Deserialize this with built-in deserializer?
                var azureMetadataNode = (YamlMappingNode)rootNode.Children[new YamlScalarNode("azureMetadata")];
                azureMetadata = DeserializeAzureMetadata(azureMetadataNode);
            }

            List<MetricDefinition> metrics = null;
            if (rootNode.Children.ContainsKey("metrics"))
            {
                var metricsNode = (YamlSequenceNode)rootNode.Children[new YamlScalarNode("metrics")];
                metrics = DeserializeMetrics(metricsNode);
            }

            var metricsDeclaration = new MetricsDeclaration
            {
                AzureMetadata = azureMetadata,
                Metrics = metrics
            };

            return metricsDeclaration;
        }

        private static List<MetricDefinition> DeserializeMetrics(YamlSequenceNode metricsNode)
        {
            var metrics = new List<MetricDefinition>();
            foreach (var item in metricsNode)
            {
                var metricNode = item as YamlMappingNode;
                if (metricNode == null)
                {
                    throw new SerializationException($"Failed parsing metrics because we couldn't cast an item to {nameof(YamlMappingNode)}");
                }

                var metricDefinition = DeserializeMetric(metricNode);
                metrics.Add(metricDefinition);
            }

            return metrics;
        }

        private static MetricDefinition DeserializeMetric(YamlMappingNode metricNode)
        {
            var rawResourceType = metricNode.Children[new YamlScalarNode("resourceType")];
            var resourceType = System.Enum.Parse<ResourceType>(rawResourceType.ToString());

            MetricDefinition metricDefinition;
            switch (resourceType)
            {
                case ResourceType.ServiceBusQueue:
                    metricDefinition = DeserializeServiceBusQueueMetric(metricNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, $"No deserialization was found for {resourceType}");
            }

            return metricDefinition;
        }

        private static MetricDefinition DeserializeServiceBusQueueMetric(YamlMappingNode metricNode)
        {
            var metricDefinition = DeserializeMetricDefinition(metricNode);

            var queueName = metricNode.Children[new YamlScalarNode("queueName")];
            var namespaceName = metricNode.Children[new YamlScalarNode("namespace")];

            var serviceBusQueueMetricDefinition = new ServiceBusQueueMetricDefinition
            {
                Name = metricDefinition.Name,
                Description = metricDefinition.Description,
                QueueName = queueName?.ToString(),
                Namespace = namespaceName?.ToString(),
                AzureMetricConfiguration = metricDefinition.AzureMetricConfiguration
            };

            return serviceBusQueueMetricDefinition;
        }

        private static MetricDefinition DeserializeMetricDefinition(YamlMappingNode metricNode)
        {
            var name = metricNode.Children[new YamlScalarNode("name")];
            var description = metricNode.Children[new YamlScalarNode("description")];
            var azureMetricConfigurationNode = (YamlMappingNode)metricNode.Children[new YamlScalarNode("azureMetricConfiguration")];

            var azureMetricConfiguration = DeserializeAzureMetricConfiguration(azureMetricConfigurationNode);

            var metricDefinition = new MetricDefinition
            {
                Name = name?.ToString(),
                Description = description?.ToString(),
                AzureMetricConfiguration = azureMetricConfiguration
            };

            return metricDefinition;
        }

        private static AzureMetricConfiguration DeserializeAzureMetricConfiguration(YamlMappingNode azureMetricConfigurationNode)
        {
            var metricName = azureMetricConfigurationNode.Children[new YamlScalarNode("metricName")];
            var rawAggregation = azureMetricConfigurationNode.Children[new YamlScalarNode("aggregation")];

            Enum.TryParse(rawAggregation?.ToString(), out AggregationType aggregationType);

            var azureMetricConfiguration = new AzureMetricConfiguration
            {
                MetricName = metricName?.ToString(),
                Aggregation = aggregationType
            };

            return azureMetricConfiguration;
        }

        private static AzureMetadata DeserializeAzureMetadata(YamlMappingNode azureMetadataNode)
        {
            var tenantId = azureMetadataNode.Children[new YamlScalarNode("tenantId")];
            var subscriptionId = azureMetadataNode.Children[new YamlScalarNode("subscriptionId")];
            var resourceGroupName = azureMetadataNode.Children[new YamlScalarNode("resourceGroupName")];

            var azureMetadata = new AzureMetadata
            {
                TenantId = tenantId?.ToString(),
                SubscriptionId = subscriptionId?.ToString(),
                ResourceGroupName = resourceGroupName?.ToString(),
            };

            return azureMetadata;
        }

        public static string Serialize(MetricsDeclaration metricsDeclaration)
        {
            Guard.Guard.NotNull(metricsDeclaration, nameof(metricsDeclaration));

            var serializer = YamlSerialization.CreateSerializer();
            var rawMetricsDeclaration = serializer.Serialize(metricsDeclaration);
            return rawMetricsDeclaration;
        }
    }
}