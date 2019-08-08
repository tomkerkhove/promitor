using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class NetworkInterfaceDeserializer : ResourceDeserializer
    {
        private const string NetworkInterfaceNameTag = "networkInterfaceName";

        public NetworkInterfaceDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node)
        {
            return new NetworkInterfaceResourceV2
            {
                NetworkInterfaceName = GetString(node, NetworkInterfaceNameTag)
            };
        }
    }
}
