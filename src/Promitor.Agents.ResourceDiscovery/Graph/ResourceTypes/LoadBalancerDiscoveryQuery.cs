using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class LoadBalancerDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.network/loadbalancers" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var loadBalancerName = resultRowEntry[2]?.ToString();
            var resource = new LoadBalancerResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), loadBalancerName);
            return resource;
        }
    }
}
