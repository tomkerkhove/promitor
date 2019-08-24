using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class NetworkInterfaceDeserializer : ResourceDeserializer
    {
        private const string NetworkInterfaceNameTag = "networkInterfaceName";

        public NetworkInterfaceDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var networkInterfaceName = node.GetString(NetworkInterfaceNameTag);

            return new NetworkInterfaceResourceV1
            {
                NetworkInterfaceName = networkInterfaceName
            };
        }
    }
}
