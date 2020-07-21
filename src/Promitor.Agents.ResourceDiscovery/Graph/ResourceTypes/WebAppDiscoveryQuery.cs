using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Graph.Query;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class WebAppDiscoveryQuery : AppServiceResourceDiscoveryQuery
    {
        public override GraphQueryBuilder DefineQuery(ResourceCriteriaDefinition criteria)
        {
            var graphQueryBuilder = base.DefineQuery(criteria)
                .Where("kind", Operator.DoesNotContain, "functionapp");

            return graphQueryBuilder;
        }

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var webAppName = resultRowEntry[3]?.ToString();
            var appDetails = DetermineAppDetails(webAppName);

            var resource = new WebAppResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), appDetails.AppName,appDetails.SlotName);
            return resource;
        }
    }
}
