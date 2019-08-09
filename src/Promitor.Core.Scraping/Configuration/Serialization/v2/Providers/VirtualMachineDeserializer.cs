using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class VirtualMachineDeserializer : ResourceDeserializer
    {
        private const string VirtualMachineNameTag = "virtualMachineName";

        public VirtualMachineDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node)
        {
            return new VirtualMachineResourceV2
            {
                VirtualMachineName = GetString(node, VirtualMachineNameTag)
            };
        }
    }
}
