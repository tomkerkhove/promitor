using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class GenericResourceDeserializer : ResourceDeserializer<GenericResourceV1>
    {
        private const string FilterTag = "filter";
        private const string ResourceUriTag = "resourceUri";

        public GenericResourceDeserializer(ILogger<GenericResourceDeserializer> logger) : base(logger)
        {
        }

        protected override GenericResourceV1 DeserializeResource(YamlMappingNode node)
        {
            var filter = node.GetString(FilterTag);
            var resourceUri = node.GetString(ResourceUriTag);

            return new GenericResourceV1
            {
                Filter = filter,
                ResourceUri = resourceUri
            };
        }
    }
}
