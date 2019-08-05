using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal class AggregationDeserializer : Deserializer<AggregationBuilder>
    {
        internal AggregationDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override AggregationBuilder Deserialize(YamlMappingNode node)
        {
            var aggregation = new AggregationBuilder();

            var interval = TimeSpan.FromMinutes(5);
            if (node.Children.ContainsKey("interval"))
            {
                var rawIntervalNode = node.Children[new YamlScalarNode("interval")];
                interval = TimeSpan.Parse(rawIntervalNode.ToString());                
            }
            else
            {
                Logger.LogWarning("No default aggregation was configured, falling back to {AggregationInterval}", interval.ToString("g"));
            }

            aggregation.Interval = interval;

            return aggregation;
        }
    }
}