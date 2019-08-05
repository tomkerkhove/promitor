using System;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal class MetricAggregationDeserializer : Deserializer<MetricAggregationBuilder>
    {
        private readonly YamlScalarNode _typeNode = new YamlScalarNode("type");
        private readonly YamlScalarNode _intervalNode = new YamlScalarNode("interval");

        internal MetricAggregationDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override MetricAggregationBuilder Deserialize(YamlMappingNode node)
        {
            var aggregation = new MetricAggregationBuilder();

            if (node.Children.ContainsKey(_intervalNode.Value))
            {
                var rawIntervalNode = node.Children[_intervalNode.Value];
                aggregation.Interval = TimeSpan.Parse(rawIntervalNode.ToString());
            }

            if (node.Children.ContainsKey(_typeNode.Value))
            {
                var rawTypeNode = node.Children[_typeNode];
                if (System.Enum.TryParse(rawTypeNode?.ToString(), out AggregationType aggregationType))
                {
                    aggregation.Type = aggregationType;
                }
            }

            return aggregation;
        }
    }
}