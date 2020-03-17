using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class VirtualMachineScaleSetDeserializer : ResourceDeserializer<VirtualMachineScaleSetResourceV1>
    {
        public VirtualMachineScaleSetDeserializer(ILogger<VirtualMachineScaleSetDeserializer> logger) : base(logger)
        {
            MapRequired(resource => resource.ScaleSetName);
        }
    }
}
