using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricDimensionDeserializer : Deserializer<MetricDimensionV1>
    {
        private const string NameTag = "name";

        public MetricDimensionDeserializer(ILogger<MetricDimensionDeserializer> logger)
            : base(logger)
        {
        }

        public override MetricDimensionV1 Deserialize(YamlMappingNode node)
        {
            return new MetricDimensionV1
            {
                Name = node.GetString(NameTag),
            };
        }
    }
}
