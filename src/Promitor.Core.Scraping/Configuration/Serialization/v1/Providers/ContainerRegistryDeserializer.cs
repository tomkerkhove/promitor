using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ContainerRegistryDeserializer : ResourceDeserializer<ContainerRegistryResourceV1>
    {
        private const string RegistryNameTag = "registryName";

        public ContainerRegistryDeserializer(ILogger<ContainerRegistryDeserializer> logger) : base(logger)
        {
        }

        protected override ContainerRegistryResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var registryName = node.GetString(RegistryNameTag);

            return new ContainerRegistryResourceV1
            {
                RegistryName = registryName
            };
        }
    }
}
