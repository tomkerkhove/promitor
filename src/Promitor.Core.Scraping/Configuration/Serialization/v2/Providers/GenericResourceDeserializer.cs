using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class GenericResourceDeserializer : ResourceDeserializer
    {
        private const string FilterTag = "filter";
        private const string ResourceUriTag = "resourceUri";

        public GenericResourceDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node)
        {
            return new GenericResourceV2
            {
                Filter = GetString(node, FilterTag),
                ResourceUri = GetString(node, ResourceUriTag)
            };
        }
    }
}
