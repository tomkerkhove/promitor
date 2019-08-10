using System;
using System.Collections.Generic;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal abstract class MetricDeserializer
    {
        protected ILogger Logger { get; private set; }

        internal MetricDeserializer WithLogger(ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));

            Logger = logger;
            return this;
        }

        internal abstract MetricDefinition Deserialize(YamlMappingNode metricNode);

        protected virtual TMetricDefinition DeserializeMetricDefinition<TMetricDefinition>(YamlMappingNode metricNode)
            where TMetricDefinition : MetricDefinition, new()
        {
            Guard.NotNull(metricNode, nameof(metricNode));

            var name = metricNode.Children[new YamlScalarNode("name")];
            var description = metricNode.Children[new YamlScalarNode("description")];
            
            var azureMetricConfigurationNode = (YamlMappingNode)metricNode.Children[new YamlScalarNode("azureMetricConfiguration")];

            var azureMetricConfigurationDeserializer = new AzureMetricConfigurationDeserializer(Logger);
            var azureMetricConfiguration = azureMetricConfigurationDeserializer.Deserialize(azureMetricConfigurationNode);

            var metricDefinition = new TMetricDefinition
            {
                Name = name?.ToString(),
                Description = description?.ToString(),
                AzureMetricConfiguration = azureMetricConfiguration,
                ResourceGroupName = GetResourceGroupName(metricNode)
            };

            DeserializeScraping(metricNode, metricDefinition);
            DeserializeCustomLabels(metricNode, metricDefinition);

            return metricDefinition;
        }

        private static string GetResourceGroupName(YamlMappingNode metricNode)
        {
            if (metricNode.Children.TryGetValue("resourceGroupName", out var resourceGroupNode))
            {
                return resourceGroupNode.ToString();
            }

            return null;
        }

        private static void DeserializeScraping<TMetricDefinition>(YamlMappingNode metricNode, TMetricDefinition metricDefinition) where TMetricDefinition : MetricDefinition, new()
        {
            if (metricNode.Children.ContainsKey(@"scraping") == false)
            {
                return;
            }

            var scrapingNode = (YamlMappingNode)metricNode.Children[new YamlScalarNode(@"scraping")];
            try
            {
                var scrapingIntervalNode = scrapingNode?.Children[new YamlScalarNode(@"schedule")];

                if (scrapingIntervalNode != null)
                {
                    metricDefinition.Scraping.Schedule = scrapingIntervalNode.ToString();
                }
            }
            catch (KeyNotFoundException)
            {
                // happens when the YAML doesn't have the properties in it which is fine because the object
                // will get a default interval of 'null'
            }
        }

        private static void DeserializeCustomLabels<TMetricDefinition>(YamlMappingNode metricNode, TMetricDefinition metricDefinition) where TMetricDefinition : MetricDefinition, new()
        {
            if (metricNode.Children.ContainsKey(@"labels") == false)
            {
                return;
            }

            var labelNode = (YamlMappingNode)metricNode.Children[new YamlScalarNode(@"labels")];
            foreach (KeyValuePair<YamlNode, YamlNode> customLabel in labelNode.Children)
            {
                var labelName = customLabel.Key.ToString();
                var labelValue = customLabel.Value.ToString();

                if (metricDefinition.Labels.ContainsKey(labelName))
                {
                    if (metricDefinition.Labels[labelName] == labelValue)
                    {
                        continue;
                    }

                    throw new Exception($"Label '{labelName}' is already defined with value '{metricDefinition.Labels[labelName]}' instead of '{labelValue}'");
                }

                metricDefinition.Labels.Add(labelName, labelValue);
            }
        }
    }
}
