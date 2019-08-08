using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class ContainerRegistryDeserializer : ResourceDeserializer
    {
        private const string RegistryNameTag = "registryName";

        public ContainerRegistryDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node)
        {
            return new ContainerRegistryResourceV2
            {
                RegistryName = GetString(node, RegistryNameTag)
            };
        }
    }
}
