using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ContainerInstanceDeserializer : ResourceDeserializer<ContainerInstanceResourceV1>
    {
        private const string ContainerGroupTag = "containerGroup";

        public ContainerInstanceDeserializer(ILogger<ContainerInstanceDeserializer> logger) : base(logger)
        {
        }

        protected override ContainerInstanceResourceV1 DeserializeResource(YamlMappingNode node)
        {
            var containerGroup = node.GetString(ContainerGroupTag);

            return new ContainerInstanceResourceV1
            {
                ContainerGroup = containerGroup
            };
        }
    }
}
