using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class ContainerInstanceDeserializer : Deserializer<AzureResourceDefinitionV2>
    {
        private const string ResourceGroupNameTag = "resourceGroupName";
        private const string ContainerGroupTag = "containerGroup";

        public ContainerInstanceDeserializer(ILogger logger) : base(logger)
        {
        }

        public override AzureResourceDefinitionV2 Deserialize(YamlMappingNode node)
        {
            return new ContainerInstanceResourceV2
            {
                ResourceGroupName = GetString(node, ResourceGroupNameTag),
                ContainerGroup = GetString(node, ContainerGroupTag)
            };
        }
    }
}
