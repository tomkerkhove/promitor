using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class VirtualMachineDeserializer : ResourceDeserializer
    {
        private const string VirtualMachineNameTag = "virtualMachineName";

        public VirtualMachineDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var virtualMachineName = node.GetString(VirtualMachineNameTag);

            return new VirtualMachineResourceV1
            {
                VirtualMachineName = virtualMachineName
            };
        }
    }
}
