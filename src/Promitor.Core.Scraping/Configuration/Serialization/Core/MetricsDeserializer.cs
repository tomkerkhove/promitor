using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Factories;
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

            if (Enum.TryParse<ResourceType>(rawResourceType.ToString(), out var resourceType))
            {
                return MetricDeserializerFactory
                    .GetDeserializerFor(resourceType)
                    .WithLogger(Logger)
                    .Deserialize(node);
            }
            else
            {
                throw new ArgumentException($@"Unknown 'resourceType' value in metric configuration: {rawResourceType.ToString()}");
            }
        }
    }
}