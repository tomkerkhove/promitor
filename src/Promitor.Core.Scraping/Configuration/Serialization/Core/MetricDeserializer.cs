using System;
using System.Collections.Generic;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Core
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
                AzureMetricConfiguration = azureMetricConfiguration
            };

            if (metricNode.Children.ContainsKey(@"scraping"))
            {
                var scrapingNode = (YamlMappingNode)metricNode.Children[new YamlScalarNode(@"scraping")];
                try
                {
                    var scrapingIntervalNode = scrapingNode?.Children[new YamlScalarNode(@"interval")];

                    if (scrapingIntervalNode != null)
                    {
                        metricDefinition.Scraping.Interval = TimeSpan.Parse(scrapingIntervalNode.ToString());
                    }
                }
                catch (KeyNotFoundException)
                {
                    // happens when the YAML doesn't have the properties in it which is fine because the object
                    // will get a default interval of 'null'
                }
            }

            return metricDefinition;
        }
    }
}
