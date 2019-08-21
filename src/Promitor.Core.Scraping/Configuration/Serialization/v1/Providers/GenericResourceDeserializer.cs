using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class GenericResourceDeserializer : ResourceDeserializer
    {
        private const string FilterTag = "filter";
        private const string ResourceUriTag = "resourceUri";

        public GenericResourceDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            return new GenericResourceV1
            {
                Filter = GetString(node, FilterTag),
                ResourceUri = GetString(node, ResourceUriTag)
            };
        }
    }
}
