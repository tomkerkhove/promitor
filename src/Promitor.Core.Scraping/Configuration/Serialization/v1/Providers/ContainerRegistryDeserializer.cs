using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ContainerRegistryDeserializer : ResourceDeserializer
    {
        private const string RegistryNameTag = "registryName";

        public ContainerRegistryDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            return new ContainerRegistryResourceV1
            {
                RegistryName = GetString(node, RegistryNameTag)
            };
        }
    }
}
