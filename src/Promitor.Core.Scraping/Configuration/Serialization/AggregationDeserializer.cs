using System;
using Promitor.Core.Scraping.Configuration.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    internal class AggregationDeserializer : Deserializer<Aggregation>
    {
        internal override Aggregation Deserialize(YamlMappingNode node)
        {
            var aggregation = new Aggregation();

            var interval = TimeSpan.FromMinutes(5);
            if (node.Children.ContainsKey("interval"))
            {
                var rawIntervalNode = node.Children[new YamlScalarNode("interval")];
                interval = TimeSpan.Parse(rawIntervalNode.ToString());
            }

            aggregation.Interval = interval;

            return aggregation;
        }
    }
}