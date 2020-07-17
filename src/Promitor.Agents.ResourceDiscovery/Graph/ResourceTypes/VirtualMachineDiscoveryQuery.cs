using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class VirtualMachineDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.compute/virtualmachines" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var virtualMachineName = resultRowEntry[3]?.ToString();
            
            var resource = new VirtualMachineResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), virtualMachineName);
            return resource;
        }
    }
}
