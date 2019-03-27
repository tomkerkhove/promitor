using System.Collections.Generic;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Core
{
    internal class MetricDefaultsDeserializer : Deserializer<MetricDefaults>
    {
        internal MetricDefaultsDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override MetricDefaults Deserialize(YamlMappingNode node)
        {
            Guard.NotNull(node, nameof(node));

            var metricDefaults = new MetricDefaults();

            if (node.Children.ContainsKey("aggregation"))
            {
                var metricDefaultsNode = (YamlMappingNode)node.Children[new YamlScalarNode("aggregation")];
                var metricDefaultsSerializer = new AggregationDeserializer(Logger);
                var aggregation = metricDefaultsSerializer.Deserialize(metricDefaultsNode);
                metricDefaults.Aggregation = aggregation;
            }

            if (node.Children.ContainsKey(@"scraping"))
            {
                var scrapingNode = (YamlMappingNode)node.Children[new YamlScalarNode(@"scraping")];
                try
                {
                    var scrapingIntervalNode = scrapingNode.Children[new YamlScalarNode(@"schedule")];

                    if (scrapingIntervalNode != null)
                    {
                        metricDefaults.Scraping.Schedule = scrapingIntervalNode.ToString();
                    }
                }
                catch (KeyNotFoundException)
                {
                    // happens when the YAML doesn't have the properties in it which is fine because the object
                    // will get a default interval of 'null'
                }
            }

            return metricDefaults;
        }
    }
}