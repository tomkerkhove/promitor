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
        public override string DefineQuery(ResourceCriteria criteria)
        {
            var query = GraphQueryBuilder.ForResourceType("microsoft.web/sites", "microsoft.web/sites/slots")
                .Where("kind", Operator.DoesNotContain, "functionapp")
                .WithSubscriptionsWithIds(criteria.Subscriptions) // Filter on queried subscriptions defined in landscape
                .WithResourceGroupsWithName(criteria.ResourceGroups)
                .WithinRegions(criteria.Regions)
                .WithTags(criteria.Tags)
                .Project("subscriptionId", "resourceGroup", "type", "name")
                .Build();

            return query;
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
