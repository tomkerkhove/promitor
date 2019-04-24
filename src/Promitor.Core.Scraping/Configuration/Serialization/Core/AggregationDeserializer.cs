using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Core
{
    internal class AggregationDeserializer : Deserializer<Aggregation>
    {
        internal AggregationDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override Aggregation Deserialize(YamlMappingNode node)
        {
            var aggregation = new Aggregation();

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