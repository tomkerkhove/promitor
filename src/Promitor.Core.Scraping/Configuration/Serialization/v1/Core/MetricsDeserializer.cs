using System;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Factories;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal class MetricsDeserializer : Deserializer<MetricDefinitionV1>
    {
        internal MetricsDeserializer(ILogger logger) : base(logger)
        {
        }

        public override MetricDefinitionV1 Deserialize(YamlMappingNode node)
        {
            var rawResourceType = node.Children[new YamlScalarNode("resourceType")];

            if (!System.Enum.TryParse<ResourceType>(rawResourceType.ToString(), out var resourceType))
            {
                throw new ArgumentException($@"Unknown 'resourceType' value in metric configuration: {rawResourceType}");
            }

            return MetricDeserializerFactory
                .GetDeserializerFor(resourceType)
                .WithLogger(Logger)
                .Deserialize(node);
        }
    }
}