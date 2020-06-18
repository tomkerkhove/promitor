using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class VirtualMachineDeserializer : ResourceDeserializer<VirtualMachineResourceV1>
    {
        public VirtualMachineDeserializer(ILogger<VirtualMachineDeserializer> logger) : base(logger)
        {
            Map(resource => resource.VirtualMachineName)
                .IsRequired();
        }
    }
}
