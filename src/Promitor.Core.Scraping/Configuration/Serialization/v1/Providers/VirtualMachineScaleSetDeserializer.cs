using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class VirtualMachineScaleSetDeserializer : ResourceDeserializer<VirtualMachineScaleSetResourceV1>
    {
        private const string ScaleSetNameTag = "scaleSetName";

        public VirtualMachineScaleSetDeserializer(ILogger<VirtualMachineScaleSetDeserializer> logger) : base(logger)
        {
        }

        protected override VirtualMachineScaleSetResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var scaleSetName = node.GetString(ScaleSetNameTag);

            return new VirtualMachineScaleSetResourceV1
            {
                ScaleSetName = scaleSetName
            };
        }
    }
}
