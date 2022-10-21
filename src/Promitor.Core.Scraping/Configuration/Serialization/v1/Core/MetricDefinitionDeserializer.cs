using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
﻿using System.Linq;
 using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricDefinitionDeserializer : Deserializer<MetricDefinitionV1>
    {
        private const string ResourcesTag = "resources";
        private const string ResourceDiscoveryGroupsTag = "resourceDiscoveryGroups";
        private readonly IDeserializer<AzureResourceDiscoveryGroupDefinitionV1> _azureResourceDiscoveryGroupDeserializer;
        private readonly IAzureResourceDeserializerFactory _azureResourceDeserializerFactory;

        public MetricDefinitionDeserializer(IDeserializer<AzureMetricConfigurationV1> azureMetricConfigurationDeserializer,
            IDeserializer<LogAnalyticsConfigurationV1> logAnalyticsConfigurationDeserializer,
            IDeserializer<ScrapingV1> scrapingDeserializer,
            IDeserializer<AzureResourceDiscoveryGroupDefinitionV1> azureResourceDiscoveryGroupDeserializer,
            IAzureResourceDeserializerFactory azureResourceDeserializerFactory,
            ILogger<MetricDefinitionDeserializer> logger) : base(logger)
        {
            _azureResourceDiscoveryGroupDeserializer = azureResourceDiscoveryGroupDeserializer;
            _azureResourceDeserializerFactory = azureResourceDeserializerFactory;

            Map(definition => definition.Name)
                .IsRequired();
            Map(definition => definition.Description)
                .IsRequired();
            Map(definition => definition.ResourceType)
                .IsRequired();

            Map(definition => definition.AzureMetricConfiguration)
                .MapUsingDeserializer(azureMetricConfigurationDeserializer);

            Map(definition => definition.LogAnalyticsConfiguration)
                .MapUsingDeserializer(logAnalyticsConfigurationDeserializer);

            Map(definition => definition.Labels);
            Map(definition => definition.Scraping)
                .MapUsingDeserializer(scrapingDeserializer);
            IgnoreField(ResourceDiscoveryGroupsTag);
            IgnoreField(ResourcesTag);
        }

        public override MetricDefinitionV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var metricDefinition = base.Deserialize(node, errorReporter);

            DeserializeMetrics(node, metricDefinition, errorReporter);

            return metricDefinition;
        }

        private void DeserializeMetrics(YamlMappingNode node, MetricDefinitionV1 metricDefinition, IErrorReporter errorReporter)
        {
            if (metricDefinition.ResourceType == null)
            {
                return;
            }

            var resourceTypeNode = node.Children["resourceType"];
            if (metricDefinition.ResourceType == ResourceType.NotSpecified)
            {
                errorReporter.ReportError(resourceTypeNode, "'resourceType' must not be set to 'NotSpecified'.");
                return;
            }

            if (node.Children.TryGetValue(ResourceDiscoveryGroupsTag, out var resourceDiscoveryGroupNode))
            {
                metricDefinition.ResourceDiscoveryGroups = _azureResourceDiscoveryGroupDeserializer.Deserialize((YamlSequenceNode)resourceDiscoveryGroupNode, errorReporter);
            }

            if (node.Children.TryGetValue(ResourcesTag, out var metricsNode))
            {
                var resourceDeserializer = _azureResourceDeserializerFactory.GetDeserializerFor(metricDefinition.ResourceType.Value);
                if (resourceDeserializer != null)
                {
                    metricDefinition.Resources = resourceDeserializer.Deserialize((YamlSequenceNode)metricsNode, errorReporter);
                }
                else
                {
                    errorReporter.ReportError(resourceTypeNode, $"Could not find a deserializer for resource type '{metricDefinition.ResourceType}'.");
                }
            }

            if ((metricDefinition.Resources == null || !metricDefinition.Resources.Any()) &&
                (metricDefinition.ResourceDiscoveryGroups == null || !metricDefinition.ResourceDiscoveryGroups.Any()))
            {
                errorReporter.ReportError(node, "Either 'resources' or 'resourceDiscoveryGroups' must be specified.");
            }

            ReportConfigurationError(node, metricDefinition, errorReporter);
        }

        private void ReportConfigurationError(YamlMappingNode node, MetricDefinitionV1 metricDefinition, IErrorReporter errorReporter)
        {
            if (metricDefinition.ResourceType == ResourceType.LogAnalytics)
            {
                if (metricDefinition.LogAnalyticsConfiguration == null)
                {
                    errorReporter.ReportError(node, "'logAnalyticsConfiguration' must be specified with LogAnalytics resource type");
                }
                if (metricDefinition.AzureMetricConfiguration != null)
                {
                    errorReporter.ReportWarning(node, "'azureMetricConfiguration' will be ignored with LogAnalytics resource type");
                }
            }
            else
            {
                if (metricDefinition.AzureMetricConfiguration == null)
                {
                    errorReporter.ReportError(node, "'azureMetricConfiguration' must be specified with this resource type");
                }
                if (metricDefinition.LogAnalyticsConfiguration != null)
                {
                    errorReporter.ReportWarning(node, "'logAnalyticsConfiguration' will be ignored with this resource type");
                }
            }
        }
    }
}
