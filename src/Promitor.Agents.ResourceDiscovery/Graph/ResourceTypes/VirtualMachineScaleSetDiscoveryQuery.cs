using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class VirtualMachineScaleSetDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.compute/virtualmachinescalesets" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name", "id" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var vmss = resultRowEntry[3]?.ToString();

            var resource = new VirtualMachineScaleSetResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), vmss);
            return resource;
        }
    }
}
