using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class VirtualMachineScaleSetDeserializer : ResourceDeserializer
    {
        private const string ScaleSetNameTag = "scaleSetName";

        public VirtualMachineScaleSetDeserializer(ILogger<VirtualMachineScaleSetDeserializer> logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var scaleSetName = node.GetString(ScaleSetNameTag);

            return new VirtualMachineScaleSetResourceV1
            {
                ScaleSetName = scaleSetName
            };
        }
    }
}
