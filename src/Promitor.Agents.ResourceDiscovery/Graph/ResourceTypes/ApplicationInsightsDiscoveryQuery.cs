using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class ApplicationInsightsDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.insights/components" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var applicationInsightsName = resultRowEntry[3]?.ToString();
            
            var resource = new ApplicationInsightsResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), applicationInsightsName);
            return resource;
        }
    }
}
